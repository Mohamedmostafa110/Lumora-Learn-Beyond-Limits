using Lumora.Common.Constants;
using Lumora.Common.Exceptions;
using Lumora.DTOs.Courses;
using Lumora.DTOs.Orders;
using Lumora.Extensions;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
    [AllowAnonymous]
    public class VerificationController : Controller
    {
        private readonly ICertificateVerificationService _verificationService;

        public VerificationController(ICertificateVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return View();
            }

            var result = await _verificationService.VerifyCertificateAsync(code);
            return View(result);
        }
    }

    [AllowAnonymous]
    public class CatalogController : Controller
    {
        private readonly ICourseCatalogService _catalogService;

        public CatalogController(ICourseCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? categoryId, Difficulty? difficulty, string? searchTerm)
        {
            var courses = await _catalogService.GetCatalogCoursesAsync(categoryId, difficulty, searchTerm);
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var course = await _catalogService.GetCourseDetailsAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }
    }

    [Authorize(Roles = PlatformRoles.Student)]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ICourseCatalogService _catalogService;
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutController(
            ICheckoutService checkoutService,
            ICourseCatalogService catalogService,
            IUnitOfWork unitOfWork)
        {
            _checkoutService = checkoutService;
            _catalogService = catalogService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? courseId)
        {
            CourseDetailDto? course = null;
            if (!string.IsNullOrEmpty(courseId))
            {
                course = await _catalogService.GetCourseDetailsAsync(courseId);
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process([FromForm] CheckoutRequestDto request)
        {
            var userId = User.GetUserId();
            var student = string.IsNullOrEmpty(userId)
                ? null
                : await _unitOfWork.Students.FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return Forbid();
            }

            request.StudentId = student.StudentId;

            try
            {
                var summary = await _checkoutService.ProcessCheckoutAsync(request);
                return RedirectToAction("Confirmation", new { orderId = summary.OrderId });
            }
            catch (DuplicatePurchaseException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Catalog");
            }
            catch (InvalidCouponException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Catalog");
            }
        }

        [HttpGet]
        public IActionResult Confirmation(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }

    [Authorize(Roles = PlatformRoles.Student)]
    public class StudentController : Controller
    {
        private readonly IDashboardService _dashboard;
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IDashboardService dashboard, IUnitOfWork unitOfWork)
        {
            _dashboard = dashboard;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.GetUserId();
            var student = string.IsNullOrEmpty(userId)
                ? null
                : await _unitOfWork.Students.FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return Forbid();
            }

            var model = await _dashboard.GetStudentDashboardAsync(student.StudentId);
            return View(model);
        }
    }

    [Authorize(Roles = PlatformRoles.Instructor)]
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.GetUserId();
            var instructor = string.IsNullOrEmpty(userId)
                ? null
                : await _unitOfWork.Instructors.FirstOrDefaultAsync(i => i.UserId == userId);

            if (instructor == null)
            {
                return Forbid();
            }

            var courses = await _unitOfWork.Courses.GetCoursesByInstructorAsync(instructor.InstructorId);
            return View(courses);
        }

        [HttpGet]
        public IActionResult CourseBuilder()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EngagementQna()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MediaAndQuizBuilder()
        {
            return View();
        }
    }

    [Authorize(Roles = PlatformRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminApprovalService _approvalService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IAdminApprovalService approvalService, IUnitOfWork unitOfWork)
        {
            _approvalService = approvalService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.PendingCourses = await _approvalService.GetPendingApprovalCoursesAsync();
            ViewBag.TotalOrders = await _unitOfWork.Orders.CountAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCourse(string courseId)
        {
            var userId = User.GetUserId();
            var admin = string.IsNullOrEmpty(userId)
                ? null
                : await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin == null)
            {
                return Forbid();
            }

            await _approvalService.ApproveCourseAsync(admin.AdminId, courseId);
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCourse(string courseId, string feedback)
        {
            var userId = User.GetUserId();
            var admin = string.IsNullOrEmpty(userId)
                ? null
                : await _unitOfWork.Admins.FirstOrDefaultAsync(a => a.UserId == userId);

            if (admin == null)
            {
                return Forbid();
            }

            await _approvalService.RejectCourseAsync(admin.AdminId, courseId, feedback);
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> CourseApprovals()
        {
            ViewBag.PendingCourses = await _approvalService.GetPendingApprovalCoursesAsync();
            return View();
        }

        [HttpGet]
        public IActionResult TaxonomyAndCoupons()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UsersManagement()
        {
            return View();
        }
    }
}
