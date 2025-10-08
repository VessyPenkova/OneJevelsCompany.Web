namespace OneJevelsCompany.Web.Services.Components
{
    public record AddSingleComponentDto
    {
        public int ComponentId { get; init; }
        public string Dimension { get; init; }
        public int Quantity { get; init; }
    }
}
