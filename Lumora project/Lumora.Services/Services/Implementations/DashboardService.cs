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
        public async Task<StudentDashboardDto> GetStudentDashboardAsync(string studentId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsWithProgressAsync(studentId);
            var certificatesCount = await _unitOfWork.Certificates.CountAsync(c => c.StudentId == studentId);

            var courses = new List<EnrolledCourseDto>();
            var quizzes = new List<StudentQuizDto>();
            ContinueLearningDto? continueLearning = null;
            int totalLessons = 0, totalDone = 0;

            foreach (var e in enrollments.OrderByDescending(x => x.EnrolledAt))
            {
                var completedIds = e.LessonProgresses
                    .Where(p => p.IsCompleted)
                    .Select(p => p.LessonId)
                    .ToHashSet();

                var dto = new EnrolledCourseDto
                {
                    CourseId = e.CourseId,
                    Title = e.Course.Title,
                    TotalLessons = e.Course.Lessons.Count,
                    CompletedLessons = completedIds.Count
                };
                courses.Add(dto);

                totalLessons += dto.TotalLessons;
                totalDone += dto.CompletedLessons;

                if (dto.IsCompleted) continue;

                if (continueLearning is null)
                {
                    var next = e.Course.Lessons
                        .OrderBy(l => l.SortOrder)
                        .FirstOrDefault(l => !completedIds.Contains(l.LessonId));

                    continueLearning = new ContinueLearningDto
                    {
                        CourseId = e.CourseId,
                        CourseTitle = e.Course.Title,
                        NextLessonId = next?.LessonId,
                        NextLessonTitle = next?.Title,
                        ProgressPercent = dto.ProgressPercent
                    };
                }

                quizzes.AddRange(e.Course.Quizzes.Select(q => new StudentQuizDto
                {
                    QuizId = q.QuizId,
                    Title = q.Title,
                    CourseTitle = e.Course.Title,
                    PassingScore = q.PassingScore
                }));
            }

            return new StudentDashboardDto
            {
                StudentName = student?.FullName ?? "Student",
                EnrolledCount = courses.Count,
                InProgressCount = courses.Count(c => !c.IsCompleted),
                OverallCompletionPercent = totalLessons == 0 ? 0 : (int)Math.Round(totalDone * 100.0 / totalLessons),
                LessonsCompleted = totalDone,
                CertificatesCount = certificatesCount,
                ContinueLearning = continueLearning,
                Courses = courses,
                Quizzes = quizzes.Take(3).ToList()
            };
        }
    }
}
