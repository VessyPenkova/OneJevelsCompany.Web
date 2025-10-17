namespace OneJevelsCompany.Web.Models.Admin
{
    public class CustomerVm
    {
        public string Email { get; set; } = "";
        public string? ShippingAddress { get; set; }
        public DateTime? LastOrderOn { get; set; }
        public decimal LifetimeValue { get; set; }
    }
}
