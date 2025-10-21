namespace OneJevelsCompany.Web.Models
{
    public class InventoryItemVm
    {
        public int Id { get; set; }
        public InventoryType Type { get; set; }
        public string TypeDisplay => Type == InventoryType.Jewel ? "Jewel" : "Component";

        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        // from Jewel.QuantityOnHand / Component.QuantityOnHand
        public int QuantityOnHand { get; set; }

        // Jewel.BasePrice or Component.Price (price per piece/unit)
        public decimal UnitPrice { get; set; }

        // parsed from Component.Dimensions (or left null for Jewels unless you add it)
        public decimal? SizeLengthMm { get; set; }
        public decimal? SizeWidthMm { get; set; }

        // calculated columns
        public int? MinQtyBracelet { get; set; }
        public int? MinQtyNecklace { get; set; }
        public decimal? BasePriceBracelet { get; set; }
        public decimal? BasePriceNecklace { get; set; }
    }
}
