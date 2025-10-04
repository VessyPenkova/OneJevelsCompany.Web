namespace OneJevelsCompany.Web.Models
{
    public class JewelComponent
    {
        public int JewelId { get; set; }
        public Jewel Jewel { get; set; } = default!;

        public int ComponentId { get; set; }
        public Component Component { get; set; } = default!;
    }
}
