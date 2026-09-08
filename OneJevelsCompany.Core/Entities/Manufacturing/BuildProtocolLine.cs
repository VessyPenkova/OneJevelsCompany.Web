using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Core.Entities.Manufacturing
{
    /// <summary>
    /// Component requirement for ONE set/piece in the protocol.
    /// Total consumed = QuantityPerSet * QuantityToProduce
    /// </summary>
    public class BuildProtocolLine
    {
        public int Id { get; set; }

        public int BuildProtocolId { get; set; }
        public BuildProtocol BuildProtocol { get; set; } = default!;

        // Component to consume
        public int ComponentId { get; set; }
        public Component Component { get; set; } = default!;

        // Required per ONE set/piece
        [Range(1, int.MaxValue)]
        public int QuantityPerSet { get; set; } = 1;

        // Optional UI metadata (does not affect stock math)
        [MaxLength(40)]
        public string? PieceLabel { get; set; }

        [MaxLength(120)]
        public string? Dimension { get; set; }

        [MaxLength(40)]
        public string? Color { get; set; }
    }
}