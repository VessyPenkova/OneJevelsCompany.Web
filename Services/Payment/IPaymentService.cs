namespace OneJevelsCompany.Web.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(int orderId, decimal total, string currency = "usd");
    }
}
