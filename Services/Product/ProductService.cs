using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Product
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

        public async Task<List<Component>> GetComponentsAsync(ComponentType? type = null, JewelCategory? forCategory = null)
        {
            var q = _db.Components.Include(c => c.Category).AsQueryable();
            // future place to filter by 'type'/'forCategory'
            return await q
                .OrderBy(c => c.Category == null ? 999 : c.Category.SortOrder)
                .ThenBy(c => c.Category == null ? "Other" : c.Category.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        // Respect multiplicity of ids (no Distinct) so price = sum of each bead used once per id
        public async Task<decimal> CalculateCustomPriceAsync(IEnumerable<int> componentIds)
        {
            var list = componentIds?.ToList() ?? new();
            if (list.Count == 0) return 0m;

            var counts = list.GroupBy(id => id).ToDictionary(g => g.Key, g => g.Count());
            var comps = await _db.Components.Where(c => counts.Keys.Contains(c.Id)).ToListAsync();

            decimal total = 0m;
            foreach (var c in comps)
                total += c.Price * counts[c.Id];

            return total;
        }

        public async Task<string> DescribeComponentsAsync(IEnumerable<int> componentIds)
        {
            var ids = (componentIds ?? Array.Empty<int>()).Distinct().ToArray();
            var comps = await _db.Components
                .Include(c => c.Category)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return string.Join(", ",
                comps.Select(c => $"{(c.Category?.Name ?? "Component")}: {c.Name}"));
        }

        public Task<List<Design>> GetBestDesignsAsync(JewelCategory? category = null)
        {
            var q = _db.Designs.AsQueryable();
            if (category.HasValue) q = q.Where(d => d.Category == category.Value);
            return q.OrderBy(d => d.Name).ToListAsync();
        }

        public Task<Jewel?> GetJewelAsync(int id) =>
            _db.Jewels
              .Include(j => j.Components)
                .ThenInclude(jc => jc.Component)
              .FirstOrDefaultAsync(j => j.Id == id);

        public Task<Component?> GetComponentAsync(int id) =>
            _db.Components
               .Include(c => c.Category)
               .FirstOrDefaultAsync(c => c.Id == id);
    }
}
