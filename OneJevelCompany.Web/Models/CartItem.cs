namespace OneJevelsCompany.Web.Models
{
    public class CartItem
    {
        public string Sku { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public JewelCategory Category { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public string? ComponentsSummary { get; set; }

        // Existing
        public string? ComponentIdsCsv { get; set; }
        public int? ReadyJewelId { get; set; }

        // NEW: supports ready-made collections
        public int? CollectionId { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
        //NEW
        public bool IsCustomBuild { get; set; } = false;  // pending custom
        public string? RecipeJson { get; set; }           // serialized BuildRecipe
        public string? CustomDesignName { get; set; }     // shopper-given name

    }
}
