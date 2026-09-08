using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Core.Entities
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; } = null!;

        public int? ComponentId { get; set; }

        public Component? Component { get; set; }

        public int? JewelId { get; set; }

        public Jewel? Jewel { get; set; }

        public int? CollectionId { get; set; }

        public Collection? Collection { get; set; }

        public int Quantity { get; set; }

        public decimal UnitCost { get; set; }

        [MaxLength(160)]
        public string? Note { get; set; }
    }
}