using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Component
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        public ComponentType Type { get; set; }

        public decimal Price { get; set; }

        public ICollection<JewelComponent> Jewels { get; set; } = new List<JewelComponent>();
    }
}
