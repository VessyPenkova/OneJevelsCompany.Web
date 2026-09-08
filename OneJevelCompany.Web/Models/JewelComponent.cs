using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class JewelComponent
    {
        // composite key (configured in AppDbContext): { JewelId, ComponentId }
        public int JewelId { get; set; }
        public Jewel? Jewel { get; set; }

        public int ComponentId { get; set; }
        public Component? Component { get; set; }

        // NEW: how many of this component are used per single finished jewel
        [Range(1, 10000)]
        public int QuantityPerJewel { get; set; } = 1;
    }
}
