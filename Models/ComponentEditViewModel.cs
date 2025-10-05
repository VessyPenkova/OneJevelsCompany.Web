using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OneJevelsCompany.Web.Models
{
    public class ComponentEditViewModel
    {
        public int? Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        // NEW
        [Display(Name = "Category")]
        public int ComponentCategoryId { get; set; }

        public decimal Price { get; set; }
        [MaxLength(80)] public string? Sku { get; set; }
        [MaxLength(120)] public string? Dimensions { get; set; }
        [MaxLength(40)] public string? Color { get; set; }
        [MaxLength(40)] public string? SizeLabel { get; set; }
        public int QuantityOnHand { get; set; }

        public string? CurrentImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
