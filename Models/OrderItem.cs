namespace OneJevelsCompany.Web.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public string Title { get; set; } = string.Empty; // Either ready jewel name or "Custom Necklace" etc.
        public JewelCategory Category { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Optional: store chosen components for custom builds (CSV)
        public string? ComponentsSummary { get; set; }
    }
}
