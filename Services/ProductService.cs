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

            if (category.HasValue) q = q.Where(j => j.Category == category.Value);
            return await q.OrderBy(j => j.Name).ToListAsync();
        }

        public async Task<List<Component>> GetComponentsAsync(ComponentType? type = null, JewelCategory? forCategory = null)
        {
            var q = _db.Components.AsQueryable();
            if (type.HasValue) q = q.Where(c => c.Type == type.Value);

            // Optionally filter by category rules; for this MVP we return all
            return await q.OrderBy(c => c.Type).ThenBy(c => c.Name).ToListAsync();
        }

        public async Task<decimal> CalculateCustomPriceAsync(IEnumerable<int> componentIds)
        {
            var ids = componentIds.Distinct().ToArray();
            if (!ids.Any()) return 0m;
            var sum = await _db.Components
                .Where(c => ids.Contains(c.Id))
                .SumAsync(c => c.Price);
            return sum;
        }

        public async Task<string> DescribeComponentsAsync(IEnumerable<int> componentIds)
        {
            var ids = componentIds.Distinct().ToArray();
            var comps = await _db.Components.Where(c => ids.Contains(c.Id)).ToListAsync();
            return string.Join(", ", comps.Select(c => $"{c.Type}: {c.Name}"));
        }

        public Task<List<Design>> GetBestDesignsAsync(JewelCategory? category = null)
        {
            var q = _db.Designs.AsQueryable();
            if (category.HasValue) q = q.Where(d => d.Category == category.Value);
            return q.OrderBy(d => d.Name).ToListAsync();
        }

        public Task<Jewel?> GetJewelAsync(int id) =>
            _db.Jewels.Include(j => j.Components).ThenInclude(c => c.Component)
                .FirstOrDefaultAsync(j => j.Id == id);
    }
}
