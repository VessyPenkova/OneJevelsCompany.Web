namespace OneJevelsCompany.Web.Models.Manufacturing
{
    public class BuildRecipe
    {
        public string? Name { get; set; }    // shopper’s chosen design name
        public List<BuildPiece> Pieces { get; set; } = new();
    }
}
