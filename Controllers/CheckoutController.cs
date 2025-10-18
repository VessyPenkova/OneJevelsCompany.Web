using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Routing;
using OneJevelsCompany.Web.Services.Cart;
using OneJevelsCompany.Web.Services.Inventory;
using OneJevelsCompany.Web.Services.Orders;
using OneJevelsCompany.Web.Services.Payment;

namespace OneJevelsCompany.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly IOrderService _orders;
        private readonly IPaymentService _payments;
        private readonly IInventoryService _inventory;

        public CheckoutController(ICartService cart, IOrderService orders, IPaymentService payments, IInventoryService inventory)
        {
            _cart = cart; _orders = orders; _payments = payments; _inventory = inventory;
        }

        [HttpGet("/Checkout", Name = RouteNames.Checkout.Index)]
        public IActionResult Index()
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any()) return RedirectToRoute(RouteNames.Cart.View);
            ViewBag.Total = items.Sum(i => i.LineTotal);
            return View();
        }

        [HttpPost("/Checkout/CreateOrder", Name = RouteNames.Checkout.Create)]
        public async Task<IActionResult> CreateOrder(string? email, string? address)
        {
            var items = _cart.GetCart(HttpContext);
            if (!items.Any()) return RedirectToRoute(RouteNames.Cart.View);

            var inStock = await _inventory.ValidateCartAsync(items);
            if (!inStock)
            {
                TempData["Error"] = "Some items are out of stock. Please adjust your cart.";
                return RedirectToRoute(RouteNames.Cart.View);
            }

            var order = await _orders.CreateOrderAsync(email, address, items);
            var intent = await _payments.CreateOrUpdatePaymentIntentAsync(order.Id, order.Total);
            await _orders.MarkPaidAsync(order.Id, intent.Id);

            var savedOrder = await _orders.GetAsync(order.Id);
            if (savedOrder != null) await _inventory.DecrementOnPaidOrderAsync(savedOrder);

            _cart.Clear(HttpContext);
            return RedirectToRoute(RouteNames.Checkout.Success, new { id = order.Id });
        }

        [HttpGet("/Checkout/Success/{id:int}", Name = RouteNames.Checkout.Success)]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orders.GetAsync(id);
            if (order is null) return NotFound();
            return View(order);
        }
    }
}
