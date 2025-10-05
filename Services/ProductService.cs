using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db) { _db = db; }

        public async Task<List<Jewel>> GetReadyCollectionsAsync(JewelCategory? category = null)
        {
            var q = _db.Jewels
                .Include(j => j.Components)
                    .ThenInclude(jc => jc.Component)
                .AsQueryable();

            if (category.HasValue)
                q = q.Where(j => j.Category == category.Value);

            return await q.OrderBy(j => j.Name).ToListAsync();
        }

        // NOTE: 'type' is kept in the signature to match your existing interface,
        // but it's unused now that components use dynamic categories.
        public async Task<List<Component>> GetComponentsAsync(ComponentType? type = null, JewelCategory? forCategory = null)
        {
            var q = _db.Components
                .Include(c => c.Category)
                .AsQueryable();

            // If you later want to filter per jewel category, do it here.
            // For this MVP we return all components.

            return await q
                .OrderBy(c => c.Category!.SortOrder)
                .ThenBy(c => c.Category!.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<decimal> CalculateCustomPriceAsync(IEnumerable<int> componentIds)
        {
            var ids = componentIds.Distinct().ToArray();
            if (ids.Length == 0) return 0m;

            return await _db.Components
                .Where(c => ids.Contains(c.Id))
                .SumAsync(c => c.Price);
        }

        public async Task<string> DescribeComponentsAsync(IEnumerable<int> componentIds)
        {
            var ids = componentIds.Distinct().ToArray();
            var comps = await _db.Components
                .Include(c => c.Category)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            // e.g. "Clasp: Magnetic, Bead: Pearl 6mm"
            return string.Join(", ",
                comps.Select(c => $"{(c.Category?.Name ?? "Component")}: {c.Name}"));
        }

        public Task<List<Design>> GetBestDesignsAsync(JewelCategory? category = null)
        {
            var q = _db.Designs.AsQueryable();
            if (category.HasValue)
                q = q.Where(d => d.Category == category.Value);

            return q.OrderBy(d => d.Name).ToListAsync();
        }

        public Task<Jewel?> GetJewelAsync(int id) =>
            _db.Jewels
              .Include(j => j.Components)
                .ThenInclude(jc => jc.Component)
              .FirstOrDefaultAsync(j => j.Id == id);
    }
}
