using OneJevelsCompany.Web.Models.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Admin
{
    public class BuildProtocolFormModel
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        public BuildTargetKind TargetKind { get; set; } = BuildTargetKind.Collection;

        // Only when TargetKind == Jewel
        public JewelCategory? SingleJewelCategory { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityToProduce { get; set; } = 1;

        [Range(0, double.MaxValue)]
        public decimal BasePrice { get; set; } = 0m;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Image upload
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImageUrl { get; set; }

        // Lines
        public List<BuildProtocolLineInput> Lines { get; set; } = new()
        {
            new BuildProtocolLineInput()
        };
    }
}
