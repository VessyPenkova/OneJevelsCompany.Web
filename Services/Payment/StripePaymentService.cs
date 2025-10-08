using Microsoft.Extensions.Configuration;

namespace OneJevelsCompany.Web.Services.Payment
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _cfg;
        public StripePaymentService(IConfiguration cfg) { _cfg = cfg; }

        // In production, use Stripe.NET: create PaymentIntent with amount (cents) & currency
        public Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(int orderId, decimal total, string currency = "usd")
        {
            // Dummy/trivial intent so you can wire the flow end-to-end without secrets.
            var cents = (long)Math.Round(total * 100m, 0);
            var intent = new PaymentIntent
            {
                Id = $"pi_test_{orderId}_{Guid.NewGuid():N}",
                ClientSecret = $"secret_{Guid.NewGuid():N}",
                AmountInCents = cents,
                Currency = currency,
                Status = "requires_payment_method"
            };
            return Task.FromResult(intent);
        }
    }
}
