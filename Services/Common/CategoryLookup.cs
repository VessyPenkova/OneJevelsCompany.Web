using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;

namespace OneJevelsCompany.Web.Services.Common
{
    public sealed class CategoryLookup : ICategoryLookup
    {
        private readonly AppDbContext _db;
        public CategoryLookup(AppDbContext db) => _db = db;

        public async Task<List<SelectListItem>> ComponentCategoriesAsync(int? selectedId = null, CancellationToken ct = default)
        {
            var items = await _db.ComponentCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync(ct);

            if (selectedId.HasValue)
            {
                var selected = selectedId.Value.ToString();
                items.FirstOrDefault(i => i.Value == selected)!.Selected = true;
            }

            return items;
        }
    }
}
