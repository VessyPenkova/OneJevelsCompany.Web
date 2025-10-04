using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Services;

namespace OneJevelsCompany.Web.Controllers
{
    public class DesignsController : Controller
    {
        private readonly IProductService _products;
        public DesignsController(IProductService products) { _products = products; }

        public async Task<IActionResult> Index(JewelCategory? category)
        {
            var designs = await _products.GetBestDesignsAsync(category);
            return View(designs);
        }
    }
}
