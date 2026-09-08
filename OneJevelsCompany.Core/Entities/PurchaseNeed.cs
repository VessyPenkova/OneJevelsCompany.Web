using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Core.Entities
{
    public class PurchaseNeed
    {
        public int Id { get; set; }

        [Required]
        public int ComponentId { get; set; }

        public Component Component { get; set; } = null!;

        public int NeededQty { get; set; }

        public int MinOrderQtyUsed { get; set; }

        public string? SourcesJson { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }
}