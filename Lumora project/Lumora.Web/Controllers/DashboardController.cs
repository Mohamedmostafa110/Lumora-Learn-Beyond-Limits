using Lumora.Services;
using Lumora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lumora.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboard;

        public DashboardController(IDashboardService dashboard)
        {
            _dashboard = dashboard;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dashboard.GetDashboardAsync();
            return View(model);
        }
    }
}