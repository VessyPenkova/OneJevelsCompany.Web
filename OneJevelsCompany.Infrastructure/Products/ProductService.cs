using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Core.Entities;
using OneJevelsCompany.Core.Enums;
using OneJevelsCompany.Core.Interfaces;
using OneJevelsCompany.Infrastructure.Persistence;

namespace OneJevelsCompany.Infrastructure.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Jewel>> GetReadyCollectionsAsync(
            JewelCategory? category = null)
        {
            var q = _db.Jewels
                .Include(j => j.Components)
                    .ThenInclude(jc => jc.Component)
                .AsQueryable();

            if (category.HasValue)
            {
                q = q.Where(j => j.Category == category.Value);
            }

            return await q
                .OrderBy(j => j.Name)
                .ToListAsync();
        }

        public async Task<List<Component>> GetComponentsAsync(
            ComponentType? type = null,
            JewelCategory? forCategory = null)
        {
            var q = _db.Components
                .Include(c => c.Category)
                .AsQueryable();

            if (type.HasValue)
            {
                var categoryName = type.Value.ToString();

                q = q.Where(c =>
                    c.Category != null &&
                    c.Category.Name == categoryName);
            }

            return await q
                .OrderBy(c => c.Category == null ? 999 : c.Category.SortOrder)
                .ThenBy(c => c.Category == null ? "Other" : c.Category.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<decimal> CalculateCustomPriceAsync(
            IEnumerable<int> componentIds)
        {
            var requestedIds = componentIds.ToList();

            if (requestedIds.Count == 0)
            {
                return 0m;
            }

            var uniqueIds = requestedIds
                .Distinct()
                .ToArray();

            var components = await _db.Components
                .Where(c => uniqueIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            if (components.Count != uniqueIds.Length)
            {
                throw new InvalidOperationException(
                    "One or more selected components could not be found.");
            }

            decimal total = 0m;

            foreach (var id in requestedIds)
            {
                total += components[id].Price;
            }

            return total;
        }

        public async Task<string> DescribeComponentsAsync(
            IEnumerable<int> componentIds)
        {
            var requestedIds = componentIds.ToList();

            if (requestedIds.Count == 0)
            {
                return string.Empty;
            }

            var counts = requestedIds
                .GroupBy(id => id)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());

            var ids = counts.Keys.ToArray();

            var components = await _db.Components
                .Include(c => c.Category)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            if (components.Count != ids.Length)
            {
                throw new InvalidOperationException(
                    "One or more selected components could not be found.");
            }

            return string.Join(
                ", ",
                components
                    .OrderBy(c => c.Category == null ? 999 : c.Category.SortOrder)
                    .ThenBy(c => c.Name)
                    .Select(c =>
                    {
                        var quantity = counts[c.Id];
                        var categoryName = c.Category?.Name ?? "Component";

                        return quantity > 1
                            ? $"{quantity}× {categoryName}: {c.Name}"
                            : $"{categoryName}: {c.Name}";
                    }));
        }

        public Task<List<Design>> GetBestDesignsAsync(
            JewelCategory? category = null)
        {
            var q = _db.Designs.AsQueryable();

            if (category.HasValue)
            {
                q = q.Where(d => d.Category == category.Value);
            }

            return q
                .OrderBy(d => d.Name)
                .ToListAsync();
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