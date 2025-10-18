using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Routing;

namespace OneJevelsCompany.Web.Controllers
{
    [Authorize]
    public class AdminSetupController : Controller
    {
        private readonly UserManager<IdentityUser> _users;
        private readonly RoleManager<IdentityRole> _roles;
        private readonly IWebHostEnvironment _env;

        public AdminSetupController(UserManager<IdentityUser> users, RoleManager<IdentityRole> roles, IWebHostEnvironment env)
        {
            _users = users; _roles = roles; _env = env;
        }

        [HttpGet("/AdminSetup/Elevate", Name = RouteNames.AdminSetup.Elevate)]
        public async Task<IActionResult> Elevate()
        {
            if (!_env.IsDevelopment()) return Forbid();

            const string adminRole = "Admin";
            if (!await _roles.RoleExistsAsync(adminRole))
                await _roles.CreateAsync(new IdentityRole(adminRole));

            var me = await _users.GetUserAsync(User);
            if (me == null) return Unauthorized();

            if (!await _users.IsInRoleAsync(me, adminRole))
                await _users.AddToRoleAsync(me, adminRole);

            return Content($"User '{me.Email}' is now in role '{adminRole}'. You can open /Admin.");
        }
    }
}
