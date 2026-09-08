namespace OneJevelsCompany.Core.Entities.Manufacturing
{
    public class BuildRecipe
    {
        public string? Name { get; set; }    // shopper’s chosen design name

        public List<BuildPiece> Pieces { get; set; } = new();
    }
}