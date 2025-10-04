namespace OneJevelsCompany.Web.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(int orderId, decimal total, string currency = "usd");
        // Add webhook handling endpoint in controller if using Stripe (not included here for brevity)
    }
}
