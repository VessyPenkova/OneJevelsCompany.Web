using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models.Admin
{
    public class InvoiceInputModel
    {
        [MaxLength(60)]
        public string Number { get; set; } = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}";

        [MaxLength(120)]
        public string? SupplierName { get; set; }

        public DateTime IssuedOnUtc { get; set; } = DateTime.UtcNow;

        public List<InvoiceLineInput> Lines { get; set; } = new()
        {
            new InvoiceLineInput()
        };
    }
}
