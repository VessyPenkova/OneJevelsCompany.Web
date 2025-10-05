using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public decimal BasePrice { get; set; } = 0m;
        public int QuantityOnHand { get; set; } = 0;

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
