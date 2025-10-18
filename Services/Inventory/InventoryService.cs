using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _db;
        public InventoryService(AppDbContext db) { _db = db; }

        // ---------- helpers ----------
        private static Dictionary<int, int> CountIds(string csv)
        {
            return csv.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => int.Parse(s))
                      .GroupBy(id => id)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        private async Task<bool> HasEnoughAsync(Dictionary<int, int> perCompCounts, int multiplier)
        {
            var ids = perCompCounts.Keys.ToList();
            var comps = await _db.Components
                .Where(c => ids.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            foreach (var kv in perCompCounts)
            {
                if (!comps.TryGetValue(kv.Key, out var c)) return false;
                var needed = kv.Value * multiplier;
                if (c.QuantityOnHand < needed) return false;
            }
            return true;
        }

        private async Task DecrementAsync(Dictionary<int, int> perCompCounts, int multiplier)
        {
            var ids = perCompCounts.Keys.ToList();
            var comps = await _db.Components
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            foreach (var c in comps)
            {
                var repeatsPerPiece = perCompCounts[c.Id];
                c.QuantityOnHand -= repeatsPerPiece * multiplier;
            }
        }
        // -----------------------------

        // ----- Apply inventory received via an invoice -----
        public async Task ApplyInvoiceAsync(Invoice invoice)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            _db.Invoices.Add(invoice);

            foreach (var line in invoice.Lines)
            {
                if (line.ComponentId.HasValue)
                {
                    var comp = await _db.Components.FirstAsync(c => c.Id == line.ComponentId.Value);
                    comp.QuantityOnHand += line.Quantity;
                }
                else if (line.JewelId.HasValue)
                {
                    var jewel = await _db.Jewels.FirstAsync(j => j.Id == line.JewelId.Value);
                    jewel.QuantityOnHand += line.Quantity;
                }
                else if (line.CollectionId.HasValue)
                {
                    var collection = await _db.Collections.FirstAsync(c => c.Id == line.CollectionId.Value);
                    collection.QuantityOnHand += line.Quantity;
                }
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        // ----- Validate cart has sufficient stock for each line -----
        public async Task<bool> ValidateCartAsync(IEnumerable<CartItem> items)
        {
            foreach (var i in items)
            {
                // Ready jewel
                if (i.ReadyJewelId.HasValue)
                {
                    var j = await _db.Jewels.FirstOrDefaultAsync(x => x.Id == i.ReadyJewelId.Value);
                    if (j == null || j.QuantityOnHand < i.Quantity) return false;
                    continue;
                }

                // Ready collection (optional)
                if (i.CollectionId.HasValue)
                {
                    var col = await _db.Collections.FirstOrDefaultAsync(c => c.Id == i.CollectionId.Value);
                    if (col == null || col.QuantityOnHand < i.Quantity) return false;
                    continue;
                }

                // Custom build
                if (!string.IsNullOrWhiteSpace(i.ComponentIdsCsv))
                {
                    var perComp = CountIds(i.ComponentIdsCsv!);        // repeats per piece
                    var ok = await HasEnoughAsync(perComp, i.Quantity); // finished quantity multiplier
                    if (!ok) return false;
                    continue;
                }

                // Unknown line type
                return false;
            }

            return true;
        }

        // ----- Decrement inventory when an order is paid -----
        public async Task DecrementOnPaidOrderAsync(Order order)
        {
            foreach (var item in order.Items)
            {
                if (item.ReadyJewelId.HasValue)
                {
                    var jewel = await _db.Jewels.FirstAsync(j => j.Id == item.ReadyJewelId.Value);
                    jewel.QuantityOnHand -= item.Quantity;
                    continue;
                }

                if (item.CollectionId.HasValue)
                {
                    var collection = await _db.Collections.FirstAsync(c => c.Id == item.CollectionId.Value);
                    collection.QuantityOnHand -= item.Quantity;
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(item.ComponentIdsCsv))
                {
                    var perComp = CountIds(item.ComponentIdsCsv!);
                    await DecrementAsync(perComp, item.Quantity);
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task AdjustCollectionStockAsync(int collectionId, int delta)
        {
            var collection = await _db.Collections.FirstOrDefaultAsync(c => c.Id == collectionId);
            if (collection != null)
            {
                collection.QuantityOnHand += delta;
                await _db.SaveChangesAsync();
            }
        }
    }
}
