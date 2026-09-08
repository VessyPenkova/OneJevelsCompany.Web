using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities.Manufacturing
{
    public class BuildPiece
    {
        public JewelCategory PieceType { get; set; }

        public string? Label { get; set; } // "Bracelet" / "Necklace"

        public List<BuildComponentSelection> Components { get; set; } = new();
    }
}