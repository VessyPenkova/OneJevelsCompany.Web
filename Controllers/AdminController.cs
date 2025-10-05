using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Models.Admin;
using OneJevelsCompany.Web.Services;

namespace OneJevelsCompany.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IInventoryService _inventory;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext db, IInventoryService inventory, IWebHostEnvironment env)
        {
            _db = db;
            _inventory = inventory;
            _env = env;
        }

        public IActionResult Index() => View();

        // ===== Catalog lists =====
        public async Task<IActionResult> Components()
        {
            var items = await _db.Components
                .Include(c => c.Category)
                .OrderBy(c => c.Category!.SortOrder)
                .ThenBy(c => c.Category!.Name)
                .ThenBy(c => c.Name)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Jewels()
        {
            var items = await _db.Jewels
                .OrderBy(j => j.Name)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Invoices()
        {
            var list = await _db.Invoices
                .OrderByDescending(i => i.IssuedOnUtc)
                .Include(i => i.Lines)
                .ToListAsync();

            return View(list);
        }

        // ===== New / Edit Component (with image upload) =====
        [HttpGet]
        public async Task<IActionResult> NewComponent()
        {
            await LoadCategoriesAsync();
            return View(new ComponentEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewComponent(ComponentEditViewModel vm, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(vm);
            }

            var imageUrl = await SaveImageAsync(ImageFile, "components");

            var comp = new Component
            {
                Name = vm.Name,
                ComponentCategoryId = vm.ComponentCategoryId,
                Price = vm.Price,
                Sku = vm.Sku,
                Dimensions = vm.Dimensions,
                Color = vm.Color,
                SizeLabel = vm.SizeLabel,
                QuantityOnHand = vm.QuantityOnHand,
                ImageUrl = imageUrl
            };

            _db.Components.Add(comp);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Components));
        }

        [HttpGet]
        public async Task<IActionResult> EditComponent(int id)
        {
            var c = await _db.Components.FindAsync(id);
            if (c == null) return NotFound();

            await LoadCategoriesAsync();

            var vm = new ComponentEditViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ComponentCategoryId = c.ComponentCategoryId,
                Price = c.Price,
                Sku = c.Sku,
                Dimensions = c.Dimensions,
                Color = c.Color,
                SizeLabel = c.SizeLabel,
                QuantityOnHand = c.QuantityOnHand,
                CurrentImageUrl = c.ImageUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComponent(ComponentEditViewModel vm, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(vm);
            }

            var c = await _db.Components.FindAsync(vm.Id);
            if (c == null) return NotFound();

            c.Name = vm.Name;
            c.ComponentCategoryId = vm.ComponentCategoryId;
            c.Price = vm.Price;
            c.Sku = vm.Sku;
            c.Dimensions = vm.Dimensions;
            c.Color = vm.Color;
            c.SizeLabel = vm.SizeLabel;
            c.QuantityOnHand = vm.QuantityOnHand;

            if (ImageFile != null)
                c.ImageUrl = await SaveImageAsync(ImageFile, "components");

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Components));
        }

        // ===== Create Invoice =====
        [HttpGet]
        public async Task<IActionResult> NewInvoice()
        {
            await FillSelectListsAsync();
            return View(new InvoiceInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewInvoice(InvoiceInputModel vm)
        {
            // Keep only valid lines (must pick item depending on type)
            vm.Lines = vm.Lines
                .Where(l =>
                       (l.LineType == "Component" && l.ComponentId.HasValue) ||
                       (l.LineType == "Jewel" && l.JewelId.HasValue))
                .ToList();

            if (!vm.Lines.Any())
                ModelState.AddModelError(string.Empty, "Add at least one valid line.");

            if (!ModelState.IsValid)
            {
                await FillSelectListsAsync();
                return View(vm);
            }

            var invoice = new Invoice
            {
                Number = vm.Number,
                SupplierName = vm.SupplierName,
                IssuedOnUtc = vm.IssuedOnUtc,
                TotalCost = vm.Lines.Sum(l => l.UnitCost * l.Quantity),
                Lines = vm.Lines.Select(l => new InvoiceLine
                {
                    ComponentId = l.LineType == "Component" ? l.ComponentId : null,
                    JewelId = l.LineType == "Jewel" ? l.JewelId : null,
                    Quantity = l.Quantity,
                    UnitCost = l.UnitCost,
                    Note = l.Note
                }).ToList()
            };

            await _inventory.ApplyInvoiceAsync(invoice);
            TempData["Ok"] = $"Invoice {invoice.Number} saved. Stock updated.";
            return RedirectToAction(nameof(Invoices));
        }

        // ===== Helpers =====
        private async Task LoadCategoriesAsync()
        {
            ViewBag.Categories = await _db.ComponentCategories
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
        }

        // Populate selects for invoice screen (categories + components + jewels)
        private async Task FillSelectListsAsync()
        {
            await LoadCategoriesAsync();

            // Components (pretty label)
            ViewBag.Components = await _db.Components
                .Include(c => c.Category)
                .OrderBy(c => c.Category!.SortOrder)
                .ThenBy(c => c.Category!.Name)
                .ThenBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Category!.Name} • {c.Name} (stock {c.QuantityOnHand})"
                })
                .ToListAsync();

            // Raw components (id + category id) for client-side filtering
            ViewBag.ComponentsRaw = await _db.Components
                .OrderBy(c => c.ComponentCategoryId).ThenBy(c => c.Name)
                .Select(c => new
                {
                    c.Id,
                    c.ComponentCategoryId,
                    Text = $"{c.Name} (stock {c.QuantityOnHand})"
                })
                .ToListAsync();

            // Jewels
            ViewBag.Jewels = await _db.Jewels
                .OrderBy(j => j.Name)
                .Select(j => new SelectListItem
                {
                    Value = j.Id.ToString(),
                    Text = $"{j.Name} (stock {j.QuantityOnHand})"
                })
                .ToListAsync();
        }

        private async Task<string?> SaveImageAsync(IFormFile? file, string folder)
        {
            if (file == null || file.Length == 0) return null;

            var root = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(root);

            var name = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(root, name);

            using (var stream = System.IO.File.Create(path))
                await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{name}";
        }
    }
}
