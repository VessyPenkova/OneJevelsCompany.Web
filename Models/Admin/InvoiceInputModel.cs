using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Admin
{
    public class InvoiceInputModel
    {
        [Required, MaxLength(80)]
        public string Number { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public DateTime IssuedOnUtc { get; set; } = DateTime.UtcNow;

        // Calculated on the server after POST (do not bind)
        public decimal TotalCost { get; set; }

        public List<Line> Lines { get; set; } = new() { new Line() };

        public class Line
        {
            // "Component" or "Jewel"
            [Required, RegularExpression("Component|Jewel")]
            public string LineType { get; set; } = "Component";

            // If LineType == "Component"
            public int? ComponentCategoryId { get; set; }
            public int? ComponentId { get; set; }

            // If LineType == "Jewel"
            public int? JewelId { get; set; }

            [Range(1, int.MaxValue)]
            public int Quantity { get; set; } = 1;

            // <-- THIS IS THE PRICE PER UNIT
            [Range(0, double.MaxValue)]
            [DataType(DataType.Currency)]
            public decimal UnitCost { get; set; }  // ensure this is present
        }
    }
}
