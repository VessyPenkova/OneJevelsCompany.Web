using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    // URL base = /Admin/Inventory
    [Route("Admin/[controller]")]
    public class InventoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly decimal _braceletCm;
        private readonly decimal _necklaceCm;

        public InventoryController(AppDbContext db, IOptions<ProductionLengthsOptions> opts)
        {
            _db = db;
            _braceletCm = opts.Value.BraceletLengthCm;
            _necklaceCm = opts.Value.NecklaceLengthCm;
        }

        // pieces/cm = 10 / pieceLenMm; MinQty = ceil(TargetCm * pieces/cm); Base = MinQty * UnitPrice
        private static (int? minQty, decimal? basePrice) ForLength(decimal targetCm, decimal unitPrice, decimal? pieceLenMm)
        {
            if (pieceLenMm is null or <= 0) return (null, null);
            var piecesPerCm = 10m / pieceLenMm.Value;
            var minQty = (int)decimal.Ceiling(targetCm * piecesPerCm);
            var basePrice = decimal.Round(minQty * unitPrice, 2);
            return (minQty, basePrice);
        }

        // Accepts "6x4", "6 x 4", "6mm x 4mm", "6.5×3.2"
        private static (decimal? L, decimal? W) ParseDimensions(string? dims)
        {
            if (string.IsNullOrWhiteSpace(dims)) return (null, null);

            var s = dims.ToLower().Replace("×", "x");
            s = Regex.Replace(s, @"(mm|cm|in|""|’|\"")", "");
            var m = Regex.Match(s, @"(?<!\d)(\d+(?:[.,]\d+)?)[\s]*x[\s]*(\d+(?:[.,]\d+)?)(?!\d)");
            if (!m.Success) return (null, null);

            var l = decimal.Parse(m.Groups[1].Value.Replace(',', '.'),
                System.Globalization.CultureInfo.InvariantCulture);
            var w = decimal.Parse(m.Groups[2].Value.Replace(',', '.'),
                System.Globalization.CultureInfo.InvariantCulture);
            return (l, w); // mm
        }

        // GET: /Admin/Inventory
        [HttpGet("", Name = "AdminInventory_Index")]
        public async Task<IActionResult> Index()
        {
            // Components
            var compRows = await _db.Components
                .AsNoTracking()
                .Where(c => c.QuantityOnHand > 0)
                .Select(c => new InventoryItemVm
                {
                    Id = c.Id,
                    Type = InventoryType.Component,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    QuantityOnHand = c.QuantityOnHand,
                    UnitPrice = c.Price
                })
                .ToListAsync();

            // Parse dimensions
            var compDims = await _db.Components
                .AsNoTracking()
                .Where(c => c.QuantityOnHand > 0)
                .Select(c => new { c.Id, c.Dimensions })
                .ToListAsync();

            var dimsById = compDims.ToDictionary(d => d.Id, d => d.Dimensions);
            foreach (var r in compRows)
            {
                if (dimsById.TryGetValue(r.Id, out var dimText))
                {
                    var (L, W) = ParseDimensions(dimText);
                    r.SizeLengthMm = L;
                    r.SizeWidthMm = W;
                }
            }

            // Jewels
            var jewelRows = await _db.Jewels
                .AsNoTracking()
                .Where(j => j.QuantityOnHand > 0)
                .Select(j => new InventoryItemVm
                {
                    Id = j.Id,
                    Type = InventoryType.Jewel,
                    Name = j.Name,
                    ImageUrl = j.ImageUrl,
                    QuantityOnHand = j.QuantityOnHand,
                    UnitPrice = j.BasePrice,
                    SizeLengthMm = null,
                    SizeWidthMm = null
                })
                .ToListAsync();

            var all = compRows.Concat(jewelRows).ToList();

            // Calculated columns
            foreach (var r in all)
            {
                var (minB, baseB) = ForLength(_braceletCm, r.UnitPrice, r.SizeLengthMm);
                var (minN, baseN) = ForLength(_necklaceCm, r.UnitPrice, r.SizeLengthMm);
                r.MinQtyBracelet = minB;
                r.BasePriceBracelet = baseB;
                r.MinQtyNecklace = minN;
                r.BasePriceNecklace = baseN;
            }

            var model = all.OrderBy(x => x.Type).ThenBy(x => x.Name).ToList();

            // IMPORTANT: your file is at /Views/Admin/Inventory.cshtml (not in a subfolder)
            return View("~/Views/Admin/Inventory.cshtml", model);
        }
    }
}
