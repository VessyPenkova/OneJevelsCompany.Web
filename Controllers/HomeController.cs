using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Models;

namespace OneJevelsCompany.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();     // Home
        public IActionResult About() => View();     // About Us
    }
}
