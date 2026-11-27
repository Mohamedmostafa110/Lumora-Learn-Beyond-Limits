using Lumora.Common.Constants;
using Lumora.Data.Identity;
using Lumora.Repositories.Interfaces;
using Lumora.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    return await RedirectToRoleDashboardAsync(currentUser, returnUrl);
                }
            }

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

            var email = model.Email.Trim();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email address or password.");
                return View(model);
            }

            if (user.AccountStatus == AccountStatus.Suspended)
            {
                _logger.LogWarning("Suspended account {Email} attempted to sign in.", email);
                ModelState.AddModelError(string.Empty, "This account has been suspended. Please contact support.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} signed in successfully.", email);
                return await RedirectToRoleDashboardAsync(user, returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} locked out after repeated failed sign-in attempts.", email);
                ModelState.AddModelError(string.Empty, "Account temporarily locked due to repeated failed attempts. Please try again in 15 minutes.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid email address or password.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? role = "Student")
        {
            var normalizedRole = NormalizePublicRole(role);
            return View(new RegisterViewModel { Role = normalizedRole });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Prevent privilege escalation: public registration is strictly limited to Student or Instructor.
            var assignedRole = NormalizePublicRole(model.Role);
            model.Role = assignedRole;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();
            var fullName = model.FullName.Trim();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email address already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                AccountStatus = AccountStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, assignedRole);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            try
            {
                await CreateDomainProfileAsync(user, assignedRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create domain profile for user {Email}. Rolling back Identity user.", email);
                await _userManager.DeleteAsync(user);
                ModelState.AddModelError(string.Empty, "Could not complete registration. Please verify your details and try again.");
                return View(model);
            }

            _logger.LogInformation("New user registered: {Email} with role {Role}.", email, assignedRole);
            TempData["SuccessMessage"] = "Your account has been created successfully! Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetUrl = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme);

                _logger.LogInformation("Password reset requested for {Email}.", email);
                if (_environment.IsDevelopment())
                {
                    ViewBag.DevResetLink = resetUrl;
                }
            }

            // Always show the same confirmation message to avoid revealing whether an email is registered.
            ViewBag.ResetSent = true;
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string? email, string? token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            return View(new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email.Trim());
            if (user == null)
            {
                TempData["SuccessMessage"] = "Your password has been reset. Please sign in.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset completed for {Email}.", user.Email);
                TempData["SuccessMessage"] = "Your password has been updated. Please sign in with your new password.";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User signed out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        private static string NormalizePublicRole(string? role) =>
            string.Equals(role?.Trim(), PlatformRoles.Instructor, StringComparison.OrdinalIgnoreCase)
                ? PlatformRoles.Instructor
                : PlatformRoles.Student;

        private async Task CreateDomainProfileAsync(ApplicationUser user, string role)
        {
            if (role == PlatformRoles.Instructor)
            {
                var instructor = new Instructor
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    AccountStatus = user.AccountStatus,
                    EmailVerifiedAt = user.EmailVerifiedAt
                };
                await _unitOfWork.Instructors.AddAsync(instructor);
            }
            else
            {
                var student = new Student
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    AccountStatus = user.AccountStatus,
                    EmailVerifiedAt = user.EmailVerifiedAt,
                    CreatedAt = user.CreatedAt
                };
                await _unitOfWork.Students.AddAsync(student);
            }

            await _unitOfWork.CompleteAsync();
        }

        private async Task<IActionResult> RedirectToRoleDashboardAsync(ApplicationUser user, string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(PlatformRoles.Admin))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            if (roles.Contains(PlatformRoles.Instructor))
            {
                return RedirectToAction("Dashboard", "Instructor");
            }

            return RedirectToAction("Dashboard", "Student");
        }
    }
}
