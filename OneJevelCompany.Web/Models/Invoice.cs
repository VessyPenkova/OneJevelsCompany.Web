using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [MaxLength(60)] public string Number { get; set; } = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}";
        public DateTime IssuedOnUtc { get; set; } = DateTime.UtcNow;
        [MaxLength(120)] public string? SupplierName { get; set; }
        public decimal TotalCost { get; set; }

        public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
    }
}
