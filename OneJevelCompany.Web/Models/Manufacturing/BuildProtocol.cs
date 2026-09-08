using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Manufacturing
{
    public enum BuildProtocolStatus { Draft = 0, Built = 1 }
    public enum BuildTargetKind { Jewel = 1, Collection = 2 }

    /// <summary>
    /// Protocol that consumes components and produces either:
    ///  - a Jewel (single piece), or
    ///  - a Collection (a sellable set)
    /// Admin enters component quantities per set, and target quantity to produce.
    /// </summary>
    public class BuildProtocol
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        public BuildTargetKind TargetKind { get; set; } = BuildTargetKind.Collection;

        // Only required when TargetKind == Jewel
        public JewelCategory? SingleJewelCategory { get; set; }

        // How many sets/pieces to manufacture
        public int QuantityToProduce { get; set; } = 1;

        // Manual selling price for the resulting article
        public decimal BasePrice { get; set; } = 0m;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public BuildProtocolStatus Status { get; set; } = BuildProtocolStatus.Draft;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? BuiltUtc { get; set; }

        // Lines with component requirements per ONE set
        public ICollection<BuildProtocolLine> Lines { get; set; } = new List<BuildProtocolLine>();
    }
}
