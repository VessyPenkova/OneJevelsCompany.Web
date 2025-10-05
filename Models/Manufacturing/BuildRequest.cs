namespace OneJevelsCompany.Web.Models.Manufacturing
{
    public class BuildRequest
    {
        public JewelCategory Category { get; set; }
        public List<int> ComponentIds { get; set; } = new();
    }
}
