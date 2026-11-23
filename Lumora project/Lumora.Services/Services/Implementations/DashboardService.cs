using Lumora.DTOs.Dashboard;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumora.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            // ── كل الـ queries بتتعمل بالـ parallel في نفس الوقت ────────────
            var studentsCountTask     = _unitOfWork.Students.CountAsync();
            var instructorsCountTask  = _unitOfWork.Instructors.CountAsync();
            var enrollmentsCountTask  = _unitOfWork.Enrollments.CountAsync();
            var certificatesCountTask = _unitOfWork.Certificates.CountAsync();
            var pendingCountTask      = _unitOfWork.Courses.CountAsync(c => c.Status == CourseStatus.PendingApproval);
            var publishedCountTask    = _unitOfWork.Courses.CountAsync(c => c.Status == CourseStatus.Published);

            var recentOrdersTask      = _unitOfWork.Orders.GetRecentOrdersAsync(take: 10);
            var topCoursesTask        = _unitOfWork.Courses.GetTopCoursesByEnrollmentAsync(take: 5);
            var recentEnrollmentsTask = _unitOfWork.Enrollments.GetRecentEnrollmentsAsync(take: 10);
            var monthlyRevenueTask    = _unitOfWork.Orders.GetMonthlyRevenueAsync(months: 6);
            var totalRevenueTask      = _unitOfWork.Orders.GetTotalRevenueAsync();

            // انتظر كلهم مع بعض (بدل ما نستنى كل واحد لوحده)
            await Task.WhenAll(
                studentsCountTask, instructorsCountTask,
                enrollmentsCountTask, certificatesCountTask,
                pendingCountTask, publishedCountTask,
                recentOrdersTask, topCoursesTask,
                recentEnrollmentsTask, monthlyRevenueTask,
                totalRevenueTask
            );

            return new DashboardDto
            {
                TotalStudents         = studentsCountTask.Result,
                TotalInstructors      = instructorsCountTask.Result,
                TotalEnrollments      = enrollmentsCountTask.Result,
                CertificatesIssued    = certificatesCountTask.Result,
                PendingApprovalCount  = pendingCountTask.Result,
                TotalPublishedCourses = publishedCountTask.Result,
                TotalRevenue          = totalRevenueTask.Result,
                RecentOrders          = recentOrdersTask.Result,
                TopCourses            = topCoursesTask.Result,
                RecentEnrollments     = recentEnrollmentsTask.Result,
                MonthlyRevenue        = monthlyRevenueTask.Result,
            };
        }
    }
}
