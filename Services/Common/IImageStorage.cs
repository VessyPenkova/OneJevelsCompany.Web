namespace OneJevelsCompany.Web.Services.Common
{
    public interface IImageStorage
    {
        Task<string?> SaveAsync(IFormFile? file, string folder, CancellationToken ct = default);
        Task<string?> SaveDataUrlAsync(string? dataUrl, string folder, CancellationToken ct = default);
    }
}
