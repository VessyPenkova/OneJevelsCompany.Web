using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OneJevelsCompany.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly IOrderService _orders;
        private readonly IPaymentService _payments;
        private readonly IInventoryService _inventory;

        public CheckoutController(
            ICartService cart,
            IOrderService orders,
            IPaymentService payments,
            IInventoryService inventory)
        {
            _cart = cart;
            _orders = orders;
            _payments = payments;
            _inventory = inventory;
        }

        // GET /Checkout
        [HttpGet]
        public IActionResult Index()
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any())
                return RedirectToAction("Cart", "Cart");

            ViewBag.Total = items.Sum(i => i.LineTotal);
            return View();
        }

        // POST /Checkout/CreateOrder
        [HttpPost]
        public async Task<IActionResult> CreateOrder(string? email, string? address)
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any())
                return RedirectToAction("Cart", "Cart");

            // 1) Validate inventory before placing the order
            var inStock = await _inventory.ValidateCartAsync(items);
            if (!inStock)
            {
                TempData["Error"] = "Some items are out of stock. Please adjust your cart.";
                return RedirectToAction("Cart", "Cart");
            }

            // 2) Create the order from the cart
            var order = await _orders.CreateOrderAsync(email, address, items);

            // 3) Create (or update) a payment intent (Stripe-ready abstraction)
            var intent = await _payments.CreateOrUpdatePaymentIntentAsync(order.Id, order.Total);

            // 4) For now, simulate immediate success; in production confirm via Stripe Elements/Webhooks
            await _orders.MarkPaidAsync(order.Id, intent.Id);

            // 5) Decrement inventory only after payment is marked as paid
            var savedOrder = await _orders.GetAsync(order.Id);
            if (savedOrder != null)
            {
                await _inventory.DecrementOnPaidOrderAsync(savedOrder);
            }

            // 6) Clear cart and redirect
            _cart.Clear(HttpContext);
            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // GET /Checkout/Success/{id}
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orders.GetAsync(id);
            if (order is null)
                return NotFound();

            return View(order);
        }
    }
}
