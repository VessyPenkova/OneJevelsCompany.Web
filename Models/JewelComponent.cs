using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class JewelComponent
    {
        public int JewelId { get; set; }
        public Jewel? Jewel { get; set; }

        public int ComponentId { get; set; }
        public Component? Component { get; set; }

        [Range(1, 10000)]
        public int QuantityPerJewel { get; set; } = 1;
    }
}
