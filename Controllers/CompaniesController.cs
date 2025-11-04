using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _db;
        public CompaniesController(AppDbContext db) => _db = db;

        // GET: /Admin/Companies
        [HttpGet("/Admin/Companies")]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View("~/Views/Admin/Companies/Index.cshtml", list);
        }

        // GET: /Admin/Companies/New
        [HttpGet("/Admin/Companies/New")]
        public IActionResult New()
            => View("~/Views/Admin/Companies/New.cshtml", new Company());

        // POST: /Admin/Companies/New
        [HttpPost("/Admin/Companies/New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Company vm)
        {
            if (!ModelState.IsValid) return View("~/Views/Admin/Companies/New.cshtml", vm);
            _db.Companies.Add(vm);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Companies/{id}
        [HttpGet("/Admin/Companies/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var company = await _db.Companies
                .Include(c => c.SalesInvoices)
                    .ThenInclude(i => i.Lines)
                        .ThenInclude(l => l.Article)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return NotFound();
            return View("~/Views/Admin/Companies/Details.cshtml", company);
        }
    }
}
