using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Models.Manufacturing; // ok to keep even if unused
using OneJevelsCompany.Web.Services;

namespace OneJevelsCompany.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _products;
        private readonly ICartService _cart;

        public ShopController(IProductService products, ICartService cart)
        {
            _products = products;
            _cart = cart;
        }

        // GET /Shop/Collections?category=Necklace
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Collections(JewelCategory? category)
        {
            var items = await _products.GetReadyCollectionsAsync(category);
            return View(items);
        }

        // GET /Shop/Build?category=Necklace
        // Returns components for the Build page (which posts to Cart/AddCustomRecipe)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Build(JewelCategory category = JewelCategory.Necklace)
        {
            var comps = await _products.GetComponentsAsync(type: null, forCategory: category);
            ViewBag.Category = category;
            return View(comps);
        }

        // LEGACY: POST /Shop/Build  (AJAX add-to-cart of simple custom with 1 of each component)
        // Kept for backward compatibility with the old Build page.
        // The new Build page should NOT use this; it should post to Cart/AddCustomRecipe.
        [HttpPost]
        [Obsolete("Use CartController.AddCustomRecipe (form POST) for quantity-based custom builds.")]
        public async Task<IActionResult> Build([FromBody] BuildRequest req)
        {
            if (req is null || req.ComponentIds == null || req.ComponentIds.Count == 0)
                return BadRequest("No components selected.");

            var price = await _products.CalculateCustomPriceAsync(req.ComponentIds);
            var summary = await _products.DescribeComponentsAsync(req.ComponentIds);

            var sku = $"CUSTOM-{req.Category}-{Guid.NewGuid():N}".ToUpperInvariant();

            _cart.AddToCart(HttpContext, new CartItem
            {
                Sku = sku,
                Title = $"Custom {req.Category}",
                Category = req.Category,
                UnitPrice = price,
                Quantity = 1,
                ComponentsSummary = summary,

                // carry component ids so inventory can validate/decrement later
                ComponentIdsCsv = string.Join(",", req.ComponentIds)
            });

            return Ok(new { sku, total = price, summary });
        }

        // POST /Shop/AddReady/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddReady(int id, int qty = 1)
        {
            var ready = await _products.GetJewelAsync(id);
            if (ready is null) return NotFound();

            var price = ready.TotalPrice();
            var sku = $"READY-{ready.Id}";

            _cart.AddToCart(HttpContext, new CartItem
            {
                Sku = sku,
                Title = ready.Name,
                Category = ready.Category,
                UnitPrice = price,
                Quantity = Math.Max(1, qty),
                ComponentsSummary = string.Join(", ", ready.Components.Select(c => c.Component.Name)),
                ReadyJewelId = ready.Id
            });

            return RedirectToAction("Cart", "Cart");
        }
    }
}
