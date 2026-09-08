using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneJevelsCompany.Web.Models
{
    public class DesignOrder
    {
        public int Id { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        // --- customer (optional) ---
        [MaxLength(80)] public string? CustomerName { get; set; }
        [MaxLength(160)] public string? CustomerEmail { get; set; }
        [MaxLength(40)] public string? CustomerPhone { get; set; }

        // --- basic selection ---
        public JewelCategory Category { get; set; } = JewelCategory.Bracelet;
        public int Quantity { get; set; } = 1;

        // --- studio params ---
        public decimal LengthCm { get; set; } // e.g. 18
        public int BeadMm { get; set; }       // e.g. 8

        [MaxLength(16)] public string Mode { get; set; } = "circle"; // "line" | "circle"
        public int Tilt { get; set; } = 65;
        public int Rotate { get; set; } = -10;

        // One cycle pattern, as JSON: [{componentId,count,mm,imageUrl?,name?}, ...]
        [MaxLength(16000)]
        public string PatternJson { get; set; } = "[]";

        // Totals / estimates
        public int OneCycleBeads { get; set; }
        public int CapacityEstimate { get; set; }
        public int PreviewBeads { get; set; }

        // Optional price
        public decimal? UnitPriceEstimate { get; set; }

        // Workflow state
        [MaxLength(24)]
        public string Status { get; set; } = "Pending"; // Pending/Confirmed/Built/etc.

        // PNG screenshot captured from the canvas (data URL).
        // nvarchar(max) so larger images are safe in SQL Server.
        [Column(TypeName = "nvarchar(max)")]
        public string? PreviewDataUrl { get; set; }

        // Optional extras
        [MaxLength(120)] public string? DesignName { get; set; }
        [MaxLength(4000)] public string? AdminNotes { get; set; }
    }
}
