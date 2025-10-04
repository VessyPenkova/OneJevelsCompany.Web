using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Services;

namespace OneJevelsCompany.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        public CartController(ICartService cart) { _cart = cart; }

        public IActionResult Cart()
        {
            var items = _cart.GetCart(HttpContext);
            ViewBag.Total = items.Sum(i => i.LineTotal);
            return View(items);
        }

        [HttpPost]
        public IActionResult Update(string sku, int qty)
        {
            _cart.UpdateQuantity(HttpContext, sku, qty);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult Remove(string sku)
        {
            _cart.Remove(HttpContext, sku);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cart.Clear(HttpContext);
            return RedirectToAction(nameof(Cart));
        }
    }
}
