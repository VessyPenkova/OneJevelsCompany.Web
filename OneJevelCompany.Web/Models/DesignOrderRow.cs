namespace OneJevelsCompany.Web.Models
{
    public class DesignOrderRow
    {
        public int Id { get; set; }
        public int DesignOrderId { get; set; }
        public DesignOrder DesignOrder { get; set; } = null!;

        public int ComponentId { get; set; }
        public int Count { get; set; }
        public int Mm { get; set; }
    }
}
