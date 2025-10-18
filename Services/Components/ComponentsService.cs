using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Services.Cart;

namespace OneJevelsCompany.Web.Services.Components
{
    public class ComponentsService : IComponentsService
    {
        private readonly AppDbContext _db;
        private readonly ICartService _cart;
        private readonly IHttpContextAccessor _http;

        public ComponentsService(AppDbContext db, ICartService cart, IHttpContextAccessor http)
        {
            _db = db;
            _cart = cart;
            _http = http;
        }

        public Task<Component?> GetByIdAsync(int id) =>
            _db.Components.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddSingleConfiguredToCartAsync(AddSingleComponentDto dto)
        {
            var c = await GetByIdAsync(dto.ComponentId);
            if (c is null) return;

            var sku = $"COMP-{c.Id}-{(string.IsNullOrWhiteSpace(dto.Dimension) ? "STD" : dto.Dimension)}".ToUpperInvariant();

            _cart.AddToCart(_http.HttpContext!, new CartItem
            {
                Sku = sku,
                Title = $"{c.Name} ({dto.Dimension})",
                Category = JewelCategory.Necklace,
                UnitPrice = c.Price,
                Quantity = Math.Max(1, dto.Quantity),
                ComponentsSummary = $"Dimension: {dto.Dimension}",
                ComponentIdsCsv = c.Id.ToString()
            });
        }
    }
}
