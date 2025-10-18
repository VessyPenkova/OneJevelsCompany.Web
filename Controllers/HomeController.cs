using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Routing;

namespace OneJevelsCompany.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/", Name = RouteNames.Home.Index)]
        public IActionResult Index() => View();

        [HttpGet("/About", Name = RouteNames.Home.About)]
        public IActionResult About() => View();
    }
}
