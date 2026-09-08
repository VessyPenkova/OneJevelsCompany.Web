using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Component
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        // NEW: dynamic category
        public int ComponentCategoryId { get; set; }
        public ComponentCategory? Category { get; set; }

        public decimal Price { get; set; }

        [MaxLength(80)] public string? Sku { get; set; }
        [MaxLength(500)] public string? ImageUrl { get; set; }
        [MaxLength(120)] public string? Dimensions { get; set; }
        [MaxLength(40)] public string? Color { get; set; }
        [MaxLength(40)] public string? SizeLabel { get; set; }
        public int QuantityOnHand { get; set; } = 0;

        public ICollection<JewelComponent> Jewels { get; set; } = new List<JewelComponent>();

        // NEW 
        [MaxLength(4000)]
        public string? Description { get; set; }

        // NEW
        [Range(1, 100000)]
        public int MinOrderQty { get; set; } = 120;

    }
}
