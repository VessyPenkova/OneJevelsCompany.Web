namespace OneJevelsCompany.Web.Models.Manufacturing
{
    public class BuildPiece
    {
        public JewelCategory PieceType { get; set; }
        public string? Label { get; set; } // "Bracelet" / "Necklace"
        public List<BuildComponentSelection> Components { get; set; } = new();
    }
}
