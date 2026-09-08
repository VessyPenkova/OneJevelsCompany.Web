using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities
{
    public class CartItem
    {
        public string Sku { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public JewelCategory Category { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal UnitPrice { get; set; }

        public string? ComponentsSummary { get; set; }

        public string? ComponentIdsCsv { get; set; }

        public int? ReadyJewelId { get; set; }

        public int? CollectionId { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;

        public bool IsCustomBuild { get; set; } = false;

        public string? RecipeJson { get; set; }

        public string? CustomDesignName { get; set; }
    }
}