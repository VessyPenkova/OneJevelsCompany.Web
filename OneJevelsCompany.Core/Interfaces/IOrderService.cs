using OneJevelsCompany.Core.Entities;

namespace OneJevelsCompany.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string? email, string? address, IEnumerable<CartItem> items);
        Task MarkPaidAsync(int orderId, string providerPaymentId);
        Task<Order?> GetAsync(int orderId);
    }
}
