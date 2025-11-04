using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OneJevelsCompany.Web.Models
{
    public class SalesInvoice
    {
        public int Id { get; set; }

        [MaxLength(60)]
        public string Number { get; set; } = $"S-{DateTime.UtcNow:yyyyMMddHHmmss}";
        public DateTime IssuedOnUtc { get; set; } = DateTime.UtcNow;

        // Optional buyer (structured)
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        // Optional free-text buyer
        [MaxLength(160)] public string? CustomerName { get; set; }
        [MaxLength(160)] public string? CustomerEmail { get; set; }

        [MaxLength(160)] public string? SellerUserName { get; set; } // admin user

        public decimal? ProfitPercent { get; set; }          // internal only, not printed
        public int? SourceDesignOrderId { get; set; }        // traceability to protocol

        public decimal Total { get; set; }
        public ICollection<SalesInvoiceLine> Lines { get; set; } = new List<SalesInvoiceLine>();
    }
}
