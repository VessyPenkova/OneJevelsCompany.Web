using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Controllers
{
    // Supports BOTH route prefixes:
    //   /Admin/ComponentCategories
    //   /Admin/Category
    [Authorize]
    [Route("Admin/ComponentCategories")]
    [Route("Admin/Category")]
    public class ComponentCategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public ComponentCategoriesController(AppDbContext db) => _db = db;

        // ----------------- helpers -----------------
        private async Task<int> NextSortAsync()
            => (await _db.ComponentCategories.MaxAsync(x => (int?)x.SortOrder)) ?? 0;

        private Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            name = name.Trim();
            return _db.ComponentCategories
                .AnyAsync(x => x.Name == name && (!excludeId.HasValue || x.Id != excludeId.Value));
        }
        // -------------------------------------------

        // LIST
        //   /Admin/ComponentCategories
        //   /Admin/Category
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var rows = await _db.ComponentCategories
                .Include(c => c.Components)
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
                .Select(c => new CategoryRowVm
                {
                    Id = c.Id,
                    Name = c.Name,
                    SortOrder = c.SortOrder,
                    Components = c.Components.Count
                })
                .ToListAsync();

            return View("~/Views/ComponentCategories/Index.cshtml", rows);
        }

        // CREATE (GET)
        //   /Admin/ComponentCategories/Create
        //   /Admin/Category/Create
        //   /Admin/Category/New        (friendly alias)
        [HttpGet("Create")]
        [HttpGet("/Admin/Category/New")]
        public async Task<IActionResult> Create()
        {
            var next = await NextSortAsync();
            var vm = new CategoryEditVm { SortOrder = next + 10 };
            return View("~/Views/ComponentCategories/Create.cshtml", vm);
        }

        // CREATE (POST)
        //   /Admin/ComponentCategories/Create
        //   /Admin/Category/Create
        //   /Admin/Category/New        (friendly alias)
        [HttpPost("Create")]
        [HttpPost("/Admin/Category/New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryEditVm vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/ComponentCategories/Create.cshtml", vm);

            if (await NameExistsAsync(vm.Name))
            {
                ModelState.AddModelError(nameof(vm.Name), "A category with this name already exists.");
                return View("~/Views/ComponentCategories/Create.cshtml", vm);
            }

            _db.ComponentCategories.Add(new ComponentCategory
            {
                Name = vm.Name.Trim(),
                SortOrder = vm.SortOrder
            });
            await _db.SaveChangesAsync();

            TempData["ok"] = "Category created.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        //   /Admin/ComponentCategories/Edit/{id}
        //   /Admin/Category/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _db.ComponentCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (cat == null) return NotFound();

            var vm = new CategoryEditVm
            {
                Id = cat.Id,
                Name = cat.Name,
                SortOrder = cat.SortOrder
            };
            return View("~/Views/ComponentCategories/Edit.cshtml", vm);
        }

        // EDIT (POST)
        //   /Admin/ComponentCategories/Edit/{id}
        //   /Admin/Category/Edit/{id}
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditVm vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/ComponentCategories/Edit.cshtml", vm);

            var cat = await _db.ComponentCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (cat == null) return NotFound();

            if (await NameExistsAsync(vm.Name, excludeId: id))
            {
                ModelState.AddModelError(nameof(vm.Name), "A category with this name already exists.");
                return View("~/Views/ComponentCategories/Edit.cshtml", vm);
            }

            cat.Name = vm.Name.Trim();
            cat.SortOrder = vm.SortOrder;
            await _db.SaveChangesAsync();

            TempData["ok"] = "Category saved.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (POST)
        //   /Admin/ComponentCategories/Delete/{id}
        //   /Admin/Category/Delete/{id}
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.ComponentCategories
                .Include(c => c.Components)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cat == null) return NotFound();

            if (cat.Components.Any())
            {
                TempData["err"] = "Cannot delete a category that has components.";
                return RedirectToAction(nameof(Index));
            }

            _db.ComponentCategories.Remove(cat);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }

        // -------- Short “New Category” flow used from New Invoice --------
        //   /Admin/NewCategory  (GET/POST)
        [HttpGet("/Admin/NewCategory")]
        public async Task<IActionResult> NewCategory()
        {
            var next = await NextSortAsync();
            var vm = new CategoryEditVm { SortOrder = next + 10 };
            return View("~/Views/Admin/NewCategory.cshtml", vm);
        }

        [HttpPost("/Admin/NewCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCategory(CategoryEditVm vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/NewCategory.cshtml", vm);

            if (await NameExistsAsync(vm.Name))
            {
                ModelState.AddModelError(nameof(vm.Name), "A category with this name already exists.");
                return View("~/Views/Admin/NewCategory.cshtml", vm);
            }

            _db.ComponentCategories.Add(new ComponentCategory
            {
                Name = vm.Name.Trim(),
                SortOrder = vm.SortOrder
            });
            await _db.SaveChangesAsync();

            TempData["ok"] = $"Category “{vm.Name.Trim()}” created.";
            return Redirect("/Admin/NewInvoice");
        }
        // ---------------------------------------------------------------

        // View models
        public class CategoryRowVm
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int SortOrder { get; set; }
            public int Components { get; set; }
        }

        public class CategoryEditVm
        {
            public int Id { get; set; }

            [Required, MaxLength(80)]
            public string Name { get; set; } = "";

            public int SortOrder { get; set; } = 100;
        }
    }
}
