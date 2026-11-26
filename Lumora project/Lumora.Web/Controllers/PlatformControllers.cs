using Lumora.Common.Exceptions;
using Lumora.DTOs.Orders;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
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

    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process([FromForm] CheckoutRequestDto request)
        {
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
        public IActionResult Confirmation(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }

    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string studentId = "1")
        {
            var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsWithProgressAsync(studentId);
            var certificates = await _unitOfWork.Certificates.GetCertificatesByStudentAsync(studentId);
            ViewBag.Certificates = certificates;
            return View(enrollments);
        }
    }

    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string instructorId = "")
        {
            var courses = await _unitOfWork.Courses.GetCoursesByInstructorAsync(instructorId);
            return View(courses);
        }
        public IActionResult CourseBuilder()
        {
            return View();
        }
        public IActionResult EngagementQna()
        {
            return View();
        }
        public IActionResult MediaAndQuizBuilder()
        {
            return View();
        }
    }

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
        public async Task<IActionResult> ApproveCourse(string adminId, string courseId)
        {
            await _approvalService.ApproveCourseAsync(adminId, courseId);
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCourse(string adminId, string courseId, string feedback)
        {
            await _approvalService.RejectCourseAsync(adminId, courseId, feedback);
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
