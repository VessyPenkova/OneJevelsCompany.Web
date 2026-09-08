using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services.Components
{
    public interface IComponentsService
    {
        Task<Component?> GetByIdAsync(int id);
        Task AddSingleConfiguredToCartAsync(AddSingleComponentDto dto);
    }
}
