using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Jewel
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        public JewelCategory Category { get; set; }

        public decimal BasePrice { get; set; }

        // NEW
        [MaxLength(500)] public string? ImageUrl { get; set; }
        public int QuantityOnHand { get; set; } = 0;

        public ICollection<JewelComponent> Components { get; set; } = new List<JewelComponent>();

        public decimal TotalPrice() => BasePrice + Components.Sum(c => c.Component?.Price ?? 0m);
    }
}
