using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities
{
    public class DesignOrder
    {
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [MaxLength(80)]
        public string? CustomerName { get; set; }

        [MaxLength(160)]
        public string? CustomerEmail { get; set; }

        [MaxLength(40)]
        public string? CustomerPhone { get; set; }

        public JewelCategory Category { get; set; }
            = JewelCategory.Bracelet;

        public int Quantity { get; set; } = 1;

        public decimal LengthCm { get; set; }

        public int BeadMm { get; set; }

        [MaxLength(16)]
        public string Mode { get; set; } = "circle";

        public int Tilt { get; set; } = 65;

        public int Rotate { get; set; } = -10;

        [MaxLength(16000)]
        public string PatternJson { get; set; } = "[]";

        public int OneCycleBeads { get; set; }

        public int CapacityEstimate { get; set; }

        public int PreviewBeads { get; set; }

        public decimal? UnitPriceEstimate { get; set; }

        [MaxLength(24)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "nvarchar(max)")]
        public string? PreviewDataUrl { get; set; }

        [MaxLength(120)]
        public string? DesignName { get; set; }

        [MaxLength(4000)]
        public string? AdminNotes { get; set; }
    }
}