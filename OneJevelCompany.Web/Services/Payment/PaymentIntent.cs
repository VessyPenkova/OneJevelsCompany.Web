namespace OneJevelsCompany.Web.Services.Payment
{
    public class PaymentIntent
    {
        public string Id { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public long AmountInCents { get; set; }
        public string Currency { get; set; } = "usd";
        public string Status { get; set; } = "requires_payment_method";
    }
}
