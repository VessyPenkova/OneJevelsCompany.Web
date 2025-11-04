using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;    // e.g., “Onyx Serenity Bracelet”
        [MaxLength(60)] public string? Category { get; set; } // Bracelet / Necklace / etc.
        [MaxLength(500)] public string? ImageUrl { get; set; }

        // From protocol (for pricing), not shown on invoice
        public decimal MaterialsCostPerPiece { get; set; }
        public decimal? DefaultMarkupPercent { get; set; }
        public decimal? DefaultSellingPrice { get; set; }
    }
}
