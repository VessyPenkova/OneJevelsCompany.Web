using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Routing;
using OneJevelsCompany.Web.Services.Cart;
using System.Text.Json;

namespace OneJevelsCompany.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        private readonly AppDbContext _db;

        public CartController(ICartService cart, AppDbContext db)
        {
            _cart = cart;
            _db = db;
        }

        // ===== Cart screen =====
        [HttpGet("/Cart", Name = RouteNames.Cart.View)]
        public IActionResult Cart()
        {
            var items = _cart.GetCart(HttpContext);
            ViewBag.Total = items.Sum(i => i.LineTotal);
            return View(items);
        }

        [HttpPost("/Cart/Update", Name = RouteNames.Cart.Update)]
        public IActionResult Update(string sku, int qty)
        {
            _cart.UpdateQuantity(HttpContext, sku, qty);
            return RedirectToRoute(RouteNames.Cart.View);
        }

        [HttpPost("/Cart/Remove", Name = RouteNames.Cart.Remove)]
        public IActionResult Remove(string sku)
        {
            _cart.Remove(HttpContext, sku);
            return RedirectToRoute(RouteNames.Cart.View);
        }

        [HttpPost("/Cart/Clear", Name = RouteNames.Cart.Clear)]
        public IActionResult Clear()
        {
            _cart.Clear(HttpContext);
            return RedirectToRoute(RouteNames.Cart.View);
        }

        // ===== Add a custom-built piece =====
        [HttpPost("/Cart/AddCustomRecipe", Name = RouteNames.Cart.AddCustomRecipe)]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddCustomRecipe(
            JewelCategory Category,
            int Quantity,
            decimal LaborPerPiece,
            string? DesignName)
        {
            var picks = new List<(int id, int qty)>();
            foreach (var key in Request.Form.Keys)
            {
                if (!key.StartsWith("Components[") || !key.EndsWith("].Quantity")) continue;
                var i1 = key.IndexOf('[') + 1;
                var i2 = key.IndexOf(']', i1);
                if (i1 <= 0 || i2 <= i1) continue;
                if (!int.TryParse(key.Substring(i1, i2 - i1), out var compId)) continue;
                if (!int.TryParse(Request.Form[key], out var qty)) qty = 0;
                if (qty > 0) picks.Add((compId, qty));
            }

            if (Quantity < 1 || picks.Count == 0)
            {
                TempData["Err"] = "Enter at least one component quantity and a valid finished quantity.";
                return RedirectToRoute(RouteNames.Shop.BuildGet, new { category = Category });
            }

            var ids = picks.Select(p => p.id).ToList();
            var comps = await _db.Components.Where(c => ids.Contains(c.Id)).ToListAsync();
            if (!comps.Any())
            {
                TempData["Err"] = "Selected components not found.";
                return RedirectToRoute(RouteNames.Shop.BuildGet, new { category = Category });
            }

            decimal materials = 0m;
            foreach (var pick in picks)
            {
                var c = comps.First(x => x.Id == pick.id);
                materials += c.Price * pick.qty;
            }
            if (LaborPerPiece < 0) LaborPerPiece = 0;
            var unitPrice = materials + LaborPerPiece;

            var compsById = comps.ToDictionary(c => c.Id, c => c);
            var summary = string.Join(", ", picks.Select(p => $"{p.qty}× {compsById[p.id].Name}"));
            var csv = string.Join(",", picks.SelectMany(p => Enumerable.Repeat(p.id, p.qty)));

            var item = new CartItem
            {
                Sku = $"CUST-{Guid.NewGuid():N}".Substring(0, 12),
                Title = $"{(string.IsNullOrWhiteSpace(DesignName) ? "Custom" : DesignName!.Trim())} ({Category})",
                Category = Category,
                Quantity = Quantity,
                UnitPrice = unitPrice,
                ComponentsSummary = summary,
                ComponentIdsCsv = csv
            };

            _cart.AddToCart(HttpContext, item);
            return RedirectToRoute(RouteNames.Cart.View);
        }
    }
}
