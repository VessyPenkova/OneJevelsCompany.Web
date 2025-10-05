using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = default!;

        // What was purchased (exactly one of the three may be set)
        public int? ComponentId { get; set; }
        public Component? Component { get; set; }

        public int? JewelId { get; set; }
        public Jewel? Jewel { get; set; }

        // NEW: allow invoicing ready-made collections
        public int? CollectionId { get; set; }
        public Collection? Collection { get; set; }

        // Amounts
        public int Quantity { get; set; }
        [Precision(14, 2)]
        public decimal UnitCost { get; set; }

        [MaxLength(160)]
        public string? Note { get; set; }
    }
}
