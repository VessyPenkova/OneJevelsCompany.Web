using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Admin
{
    public class InvoiceLineInput
    {
        // "Component" or "Jewel" or "Collection"
        public string LineType { get; set; } = "Component";

        public int? ComponentId { get; set; }
        public int? JewelId { get; set; }
        public int? CollectionId { get; set; }   // NEW

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Range(0, double.MaxValue)]
        public decimal UnitCost { get; set; } = 0m;

        [MaxLength(160)]
        public string? Note { get; set; }
    }
}
