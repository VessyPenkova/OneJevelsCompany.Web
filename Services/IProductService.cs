using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Services
{
    public interface IProductService
    {
        Task<List<Jewel>> GetReadyCollectionsAsync(JewelCategory? category = null);
        Task<List<Component>> GetComponentsAsync(ComponentType? type = null, JewelCategory? forCategory = null);
        Task<decimal> CalculateCustomPriceAsync(IEnumerable<int> componentIds);
        Task<string> DescribeComponentsAsync(IEnumerable<int> componentIds);
        Task<List<Design>> GetBestDesignsAsync(JewelCategory? category = null);
        Task<Jewel?> GetJewelAsync(int id);
    }
}
