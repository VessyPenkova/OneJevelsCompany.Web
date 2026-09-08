using System.ComponentModel.DataAnnotations;
using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities
{
    public class Design
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; } = string.Empty;

        public JewelCategory Category { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}