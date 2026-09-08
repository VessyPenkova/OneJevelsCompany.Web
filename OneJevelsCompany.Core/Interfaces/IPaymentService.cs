using OneJevelsCompany.Core.ValueObjects;

namespace OneJevelsCompany.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(
            int orderId,
            decimal total,
            string currency = "usd");
    }
}