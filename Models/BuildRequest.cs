namespace OneJevelsCompany.Web.Models
{
    public class BuildRequest
    {
        public JewelCategory Category { get; set; }
        public List<int> ComponentIds { get; set; } = new();
    }
}
