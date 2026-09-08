using OneJevelsCompany.Core.Entities;

namespace OneJevelsCompany.Core.Interfaces
{
    public interface IComponentsService
    {
        Task<Component?> GetByIdAsync(int id);
        Task AddSingleConfiguredToCartAsync(AddSingleComponentDto dto);
    }
}