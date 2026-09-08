using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public JewelCategory Category { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string? ComponentsSummary { get; set; }

        public string? ComponentIdsCsv { get; set; }

        public int? ReadyJewelId { get; set; }

        public int? CollectionId { get; set; }

        public bool IsCustomBuild { get; set; } = false;

        public string? RecipeJson { get; set; }

        public string? CustomDesignName { get; set; }
    }
}