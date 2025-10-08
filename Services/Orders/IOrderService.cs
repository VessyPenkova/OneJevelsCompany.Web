using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Orders
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string? email, string? address, IEnumerable<CartItem> items);
        Task MarkPaidAsync(int orderId, string providerPaymentId);
        Task<Order?> GetAsync(int orderId);
    }
}
