using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneJevelsCompany.Web.Routing;
using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signIn;
        private readonly UserManager<IdentityUser> _users;

        public AccountController(SignInManager<IdentityUser> signIn, UserManager<IdentityUser> users)
        {
            _signIn = signIn;
            _users = users;
        }

        // ===== LOGIN =====
        [HttpGet("/Account/Login", Name = RouteNames.Account.Login)]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // ✅ Show friendly info ONLY when user was trying to open Build Your Own
            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                returnUrl.StartsWith("/Build", StringComparison.OrdinalIgnoreCase))
            {
                ViewData["InfoMessage"] = "To use “Build your own”, you need to register or login.";
            }

            return View(new LoginViewModel());
        }

        [HttpPost("/Account/Login", Name = RouteNames.Account.LoginPost)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View("Login", model);

            var result = await _signIn.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToRoute(RouteNames.Home.Index);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Login", model);
        }

        // ===== REGISTER =====
        [HttpGet("/Account/Register", Name = RouteNames.Account.Register)]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }

        [HttpPost("/Account/Register", Name = RouteNames.Account.RegisterPost)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View("Register", model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _users.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signIn.SignInAsync(user, isPersistent: false);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToRoute(RouteNames.Home.Index);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Register", model);
        }

        // ===== LOGOUT =====
        [HttpPost("/Account/Logout", Name = RouteNames.Account.Logout)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToRoute(RouteNames.Home.Index);
        }

        // Optional: GET version for direct links
        [HttpGet("/Account/Logout", Name = RouteNames.Account.LogoutGet)]
        public async Task<IActionResult> LogoutGet()
        {
            await _signIn.SignOutAsync();
            return RedirectToRoute(RouteNames.Home.Index);
        }

        // ===== ACCESS DENIED =====
        [HttpGet("/Account/AccessDenied", Name = RouteNames.Account.AccessDenied)]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

    // ===== View Models =====
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
