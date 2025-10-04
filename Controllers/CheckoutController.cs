using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Services;

namespace OneJevelsCompany.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly IOrderService _orders;
        private readonly IPaymentService _payments;

        public CheckoutController(ICartService cart, IOrderService orders, IPaymentService payments)
        {
            _cart = cart;
            _orders = orders;
            _payments = payments;
        }

        // GET /Checkout
        [HttpGet]
        public IActionResult Index()
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any()) return RedirectToAction("Cart", "Cart");
            ViewBag.Total = items.Sum(i => i.LineTotal);
            return View();
        }

        // POST /Checkout/CreateOrder
        [HttpPost]
        public async Task<IActionResult> CreateOrder(string? email, string? address)
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any()) return RedirectToAction("Cart", "Cart");

            var order = await _orders.CreateOrderAsync(email, address, items);
            var intent = await _payments.CreateOrUpdatePaymentIntentAsync(order.Id, order.Total);

            // Normally, return client secret to JS to confirm card (Stripe Elements)
            // For now, simulate immediate payment success and clear cart:
            await _orders.MarkPaidAsync(order.Id, intent.Id);
            _cart.Clear(HttpContext);

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // GET /Checkout/Success/{id}
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orders.GetAsync(id);
            if (order is null) return NotFound();
            return View(order);
        }
    }
}
