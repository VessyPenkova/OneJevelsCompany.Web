using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using System.ComponentModel.DataAnnotations;
using OneJevelsCompany.Web.Routing;

namespace OneJevelsCompany.Web.Controllers
{
    [Authorize]
    [Route("Admin/Components")]
    public class ComponentsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ComponentsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ===== helpers
        private async Task<List<SelectListItem>> CategorySelectListAsync(int? selectedId = null)
        {
            var items = await _db.ComponentCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            if (selectedId.HasValue)
                items.Where(i => i.Value == selectedId.Value.ToString())
                     .ToList()
                     .ForEach(i => i.Selected = true);

            return items;
        }

        // ===================== CANONICAL: /Admin/Components/New =====================
        [HttpGet("New", Name = RouteNames.Components.New)]
        public async Task<IActionResult> New()
        {
            ViewBag.Categories = await CategorySelectListAsync();
            return View("~/Views/Admin/NewComponent.cshtml", new AdminNewComponentVm());
        }

        // ===================== LEGACY: /Admin/NewComponent -> 301 =====================
        [HttpGet("/Admin/NewComponent")]
        public IActionResult New_LegacyRedirect()
            => RedirectToRoutePermanent(RouteNames.Components.New);

        // ===================== POST create =====================
        [HttpPost("New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(AdminNewComponentVm vm, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await CategorySelectListAsync(vm.ComponentCategoryId);
                return View("~/Views/Admin/NewComponent.cshtml", vm);
            }

            var categoryExists = await _db.ComponentCategories.AnyAsync(c => c.Id == vm.ComponentCategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(vm.ComponentCategoryId), "Category not found.");
                ViewBag.Categories = await CategorySelectListAsync();
                return View("~/Views/Admin/NewComponent.cshtml", vm);
            }

            bool duplicate = await _db.Components.AnyAsync(c =>
                c.ComponentCategoryId == vm.ComponentCategoryId &&
                c.Name == vm.Name.Trim());

            if (duplicate)
            {
                ModelState.AddModelError(nameof(vm.Name),
                    "An item with this name already exists in the selected category.");
                ViewBag.Categories = await CategorySelectListAsync(vm.ComponentCategoryId);
                return View("~/Views/Admin/NewComponent.cshtml", vm);
            }

            string? imageUrl = null;
            if (image is { Length: > 0 })
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "components");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
                await using var fs = System.IO.File.Create(Path.Combine(uploads, fileName));
                await image.CopyToAsync(fs);
                imageUrl = $"/uploads/components/{fileName}";
            }

            var comp = new Component
            {
                Name = vm.Name.Trim(),
                ComponentCategoryId = vm.ComponentCategoryId,
                Price = vm.Price,
                Sku = vm.Sku,
                Color = vm.Color,
                SizeLabel = vm.SizeLabel,
                Dimensions = vm.Dimensions,
                ImageUrl = imageUrl,
                QuantityOnHand = 0
            };

            _db.Components.Add(comp);
            await _db.SaveChangesAsync();

            TempData["ok"] = $"Item “{comp.Name}” created.";
            return RedirectToRoute(RouteNames.Admin.NewInvoice);
        }

        // ===================== Edit =====================
        [HttpGet("Edit/{id:int}", Name = RouteNames.Components.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Components.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();

            var vm = new AdminComponentEditVm
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                QuantityOnHand = c.QuantityOnHand,
                Sku = c.Sku,
                Color = c.Color,
                SizeLabel = c.SizeLabel,
                Dimensions = c.Dimensions,
                CurrentImageUrl = c.ImageUrl,
                Description = c.Description
            };

            return View("~/Views/Components/Edit.cshtml", vm);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminComponentEditVm vm, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                vm.CurrentImageUrl = await _db.Components.Where(x => x.Id == id).Select(x => x.ImageUrl).FirstOrDefaultAsync();
                return View("~/Views/Components/Edit.cshtml", vm);
            }

            var c = await _db.Components.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();

            if (image is { Length: > 0 })
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "components");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
                await using var fs = System.IO.File.Create(Path.Combine(uploads, fileName));
                await image.CopyToAsync(fs);
                c.ImageUrl = $"/uploads/components/{fileName}";
            }

            c.Name = vm.Name.Trim();
            c.Price = vm.Price;
            c.QuantityOnHand = vm.QuantityOnHand;
            c.Sku = vm.Sku;
            c.Color = vm.Color;
            c.SizeLabel = vm.SizeLabel;
            c.Dimensions = vm.Dimensions;
            c.Description = vm.Description;

            await _db.SaveChangesAsync();
            TempData["ok"] = "Component saved.";
            return RedirectToRoute(RouteNames.Admin.ComponentsList);
        }

        // ======= VMs (unchanged)
        public class AdminNewComponentVm
        {
            [Required, MaxLength(160)] public string Name { get; set; } = string.Empty;
            [Required] public int ComponentCategoryId { get; set; }
            [Range(0, double.MaxValue)] public decimal Price { get; set; }
            [MaxLength(80)] public string? Sku { get; set; }
            [MaxLength(40)] public string? Color { get; set; }
            [MaxLength(40)] public string? SizeLabel { get; set; }
            [MaxLength(120)] public string? Dimensions { get; set; }
        }

        public class AdminComponentEditVm
        {
            public int Id { get; set; }
            [Required, MaxLength(160)] public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int QuantityOnHand { get; set; }
            [MaxLength(80)] public string? Sku { get; set; }
            [MaxLength(40)] public string? Color { get; set; }
            [MaxLength(40)] public string? SizeLabel { get; set; }
            [MaxLength(120)] public string? Dimensions { get; set; }
            public string? CurrentImageUrl { get; set; }
            [MaxLength(4000)] public string? Description { get; set; }
        }
    }
}
