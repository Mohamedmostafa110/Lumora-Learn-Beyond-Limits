using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseCatalogService _catalogService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ICourseCatalogService catalogService, IUnitOfWork unitOfWork)
        {
            _catalogService = catalogService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalStudents = await _unitOfWork.Students.CountAsync();
            ViewBag.TotalInstructors = await _unitOfWork.Instructors.CountAsync();
            ViewBag.TotalCourses = await _unitOfWork.Courses.CountAsync(c => c.Status == CourseStatus.Published);
            ViewBag.TotalCertificates = await _unitOfWork.Certificates.CountAsync();

            var featuredCourses = await _catalogService.GetCatalogCoursesAsync();
            return View(featuredCourses);
        }
    }
}
