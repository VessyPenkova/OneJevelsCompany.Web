using Microsoft.AspNetCore.Mvc.Rendering;

namespace OneJevelsCompany.Web.Services.Common
{
    public interface ICategoryLookup
    {
        Task<List<SelectListItem>> ComponentCategoriesAsync(int? selectedId = null, CancellationToken ct = default);
    }
}
