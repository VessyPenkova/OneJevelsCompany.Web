using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Order
    {
        public int Id { get; set; }

        [MaxLength(160)]
        public string? CustomerEmail { get; set; }

        [MaxLength(200)]
        public string? ShippingAddress { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public decimal Total { get; set; }

        public string Status { get; set; } = "Pending";

        public string? PaymentProviderId { get; set; } // e.g., Stripe PaymentIntent Id

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
