using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(40)] public string? VatNumber { get; set; }   // VAT
        [MaxLength(40)] public string? Bulstat { get; set; }     // Company ID (Bulstat)
        [MaxLength(160)] public string? ContactPerson { get; set; }
        [MaxLength(300)] public string? Address { get; set; }
        [MaxLength(60)] public string? Phone { get; set; }
        [MaxLength(160)] public string? Email { get; set; }

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}
