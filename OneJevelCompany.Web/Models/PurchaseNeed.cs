using System;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    /// <summary>
    /// Central queue of outstanding shortages, independent of supplier/invoice.
    /// Admin will later use this list to decide what to buy and from whom.
    /// </summary>
    public class PurchaseNeed
    {
        public int Id { get; set; }

        [Required]
        public int ComponentId { get; set; }
        public Component Component { get; set; } = null!;

        /// <summary>How many units are still needed across all sources (not yet received).</summary>
        public int NeededQty { get; set; }

        /// <summary>
        /// Snapshot of the component's MOQ when this need was (first) created.
        /// Used to compute a "SuggestedQty = ceil(Needed / MOQ) * MOQ" on the UI.
        /// </summary>
        public int MinOrderQtyUsed { get; set; }

        /// <summary>
        /// JSON with an array of { designOrderId, qtyNeeded, createdUtc } so you can see the reasons.
        /// Keep it nvarchar(max) for simplicity.
        /// </summary>
        public string? SourcesJson { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}
