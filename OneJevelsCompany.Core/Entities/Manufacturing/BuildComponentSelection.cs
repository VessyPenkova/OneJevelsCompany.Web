namespace OneJevelsCompany.Core.Entities.Manufacturing
{
    public class BuildComponentSelection
    {
        public int ComponentId { get; set; }

        public int Quantity { get; set; } = 1;

        public string? Dimension { get; set; }

        public string? Color { get; set; }
    }
}