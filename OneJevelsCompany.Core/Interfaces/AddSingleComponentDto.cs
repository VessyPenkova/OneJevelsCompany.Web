namespace OneJevelsCompany.Core.Interfaces
{
    public record AddSingleComponentDto
    {
        public int ComponentId { get; init; }

        public string Dimension { get; init; } = string.Empty;

        public int Quantity { get; init; }
    }
}