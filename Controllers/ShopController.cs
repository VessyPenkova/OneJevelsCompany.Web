using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Models;
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
        public async Task<IActionResult> Collections(JewelCategory? category)
        {
            var items = await _products.GetReadyCollectionsAsync(category);
            return View(items);
        }

        // GET /Shop/Build?category=Necklace
        [HttpGet]
        public async Task<IActionResult> Build(JewelCategory category = JewelCategory.Necklace)
        {
            // For the view: load all components (you might group by type on the UI)
            var comps = await _products.GetComponentsAsync();
            ViewBag.Category = category;
            return View(comps);
        }

        // POST /Shop/Build (AJAX/API add to cart)
        [HttpPost]
        public async Task<IActionResult> Build([FromBody] BuildRequest req)
        {
            if (req is null || req.ComponentIds.Count == 0)
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
                ComponentsSummary = summary
            });

            return Ok(new { sku, total = price, summary });
        }

        // POST /Shop/AddReady/{id}
        [HttpPost]
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
                ComponentsSummary = string.Join(", ", ready.Components.Select(c => c.Component.Name))
            });

            return RedirectToAction("Cart", "Cart");
        }
    }
}
