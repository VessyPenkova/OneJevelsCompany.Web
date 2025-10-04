namespace OneJevelsCompany.Web.Models
{
    public class CartItem
    {
        public string Sku { get; set; } = string.Empty;      // e.g., READY-<JewelId> or CUSTOM-<hash>
        public string Title { get; set; } = string.Empty;
        public JewelCategory Category { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public string? ComponentsSummary { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }

}
