using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Jewel
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        public JewelCategory Category { get; set; }

        /// <summary>
        /// For ready-made items you can keep 0; price is derived from components.
        /// </summary>
        public decimal BasePrice { get; set; }

        public ICollection<JewelComponent> Components { get; set; } = new List<JewelComponent>();

        public decimal TotalPrice() => BasePrice + Components.Sum(c => c.Component?.Price ?? 0m);
    }
}
