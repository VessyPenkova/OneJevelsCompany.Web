using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class ConfigureComponentVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        public List<string> DimensionOptions { get; set; } = new();

        [Required(ErrorMessage = "Моля изберете размер/дименсия.")]
        public string SelectedDimension { get; set; }

        [Range(1, 999, ErrorMessage = "Количеството трябва да е поне 1.")]
        public int Quantity { get; set; } = 1;

        // for rendering the dropdown limit (not posted by the browser necessarily)
        public int MaxQty { get; set; } = 20;
    }
}
