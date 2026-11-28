
using Lumora.DTOs.Student;
using Lumora.Models.Enums;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;

namespace Lumora.Services.Services.Implementations
{
    public class StudentPortalService : IStudentPortalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentPortalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<StudentCourseDto>> GetMyCoursesAsync(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException("Student ID is required.", nameof(studentId));
            }

            var enrollments = await _unitOfWork.Enrollments
                .GetStudentEnrollmentsWithProgressAsync(studentId);

            var studentCourses = enrollments
                .Where(e => e.Status != EnrollmentStatus.Cancelled)
                .OrderByDescending(e => e.EnrolledAt)
                .Select(e => new StudentCourseDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Title = e.Course.Title,
                    Difficulty = e.Course.Difficulty,
                    EnrolledAt = e.EnrolledAt,
                    Status = e.Status,
                    TotalLessons = e.Course.Lessons.Count,
                    CompletedLessons = e.LessonProgresses
                        .Where(p => p.IsCompleted)
                        .Select(p => p.LessonId)
                        .Distinct()
                        .Count()
                })
                .ToList();

            return studentCourses;
        }
    }
}
