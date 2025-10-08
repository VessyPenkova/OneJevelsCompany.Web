using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Inventory
{
    public interface IInventoryService
    {
        Task ApplyInvoiceAsync(Invoice invoice);
        Task<bool> ValidateCartAsync(IEnumerable<CartItem> items);
        Task DecrementOnPaidOrderAsync(Order order);

        // NEW: optional helper if you want direct control from AdminController
        Task AdjustCollectionStockAsync(int collectionId, int delta);
    }
}
