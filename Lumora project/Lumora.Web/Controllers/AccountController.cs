using Lumora.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("User {Email} logged into the Lumora platform.", model.Email);

            // Prototype authentication simulation: route appropriately
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (model.Email.Contains("instructor", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Dashboard", "Instructor");
            }
            if (model.Email.Contains("admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToAction("Dashboard", "Student");
        }

        [HttpGet]
        public IActionResult Register(string? role = "Student")
        {
            return View(new RegisterViewModel { Role = role ?? "Student" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("New user registration: {Email} with role {Role}", model.Email, model.Role);
            TempData["SuccessMessage"] = "Your account has been created successfully! Please sign in.";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("Password reset requested for {Email}", model.Email);
            ViewBag.ResetSent = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
