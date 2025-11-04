using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class SalesInvoiceLine
    {
        public int Id { get; set; }

        public int SalesInvoiceId { get; set; }
        public SalesInvoice SalesInvoice { get; set; } = null!;

        // Only the ready product (Article), not components
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public int Quantity { get; set; }

        [Precision(14, 2)]
        public decimal UnitPrice { get; set; }

        [MaxLength(160)]
        public string? Note { get; set; }
    }
}
