using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Inventory
{
    public interface IInventoryService
    {
        Task ApplyInvoiceAsync(Invoice invoice);
        Task<bool> ValidateCartAsync(IEnumerable<CartItem> items);
        Task DecrementOnPaidOrderAsync(Order order);
        Task AdjustCollectionStockAsync(int collectionId, int delta);
    }
}
