namespace OneJevelsCompany.Web.Models.Manufacturing
{
    public class BuildComponentSelection
    {
        public int ComponentId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Dimension { get; set; } // optional, for your 5mm, 45cm labels
        public string? Color { get; set; }
    }
}
