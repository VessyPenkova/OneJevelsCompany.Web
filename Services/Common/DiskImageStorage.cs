using Microsoft.AspNetCore.Hosting;

namespace OneJevelsCompany.Web.Services.Common
{
    public sealed class DiskImageStorage : IImageStorage
    {
        private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        private readonly IWebHostEnvironment _env;
        public DiskImageStorage(IWebHostEnvironment env) => _env = env;

        public async Task<string?> SaveAsync(IFormFile? file, string folder, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName);
            if (!Allowed.Contains(ext)) return null;

            var root = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(root);

            var name = $"{Guid.NewGuid():N}{ext}";
            await using var fs = System.IO.File.Create(Path.Combine(root, name));
            await file.CopyToAsync(fs, ct);
            return $"/uploads/{folder}/{name}";
        }

        public async Task<string?> SaveDataUrlAsync(string? dataUrl, string folder, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dataUrl)) return null;
            if (!dataUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase)) return dataUrl;

            var comma = dataUrl.IndexOf(',');
            if (comma < 0) return null;

            var base64 = dataUrl[(comma + 1)..].Replace(' ', '+');
            byte[] bytes;
            try { bytes = Convert.FromBase64String(base64); }
            catch { return null; }

            var root = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(root);

            var name = $"{Guid.NewGuid():N}.png";
            await System.IO.File.WriteAllBytesAsync(Path.Combine(root, name), bytes, ct);
            return $"/uploads/{folder}/{name}";
        }
    }
}
