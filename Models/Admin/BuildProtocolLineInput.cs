using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Admin
{
    public class BuildProtocolLineInput
    {
        [Required]
        public int? ComponentId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityPerSet { get; set; } = 1;

        public string? PieceLabel { get; set; }
        public string? Dimension { get; set; }
        public string? Color { get; set; }
    }
}
