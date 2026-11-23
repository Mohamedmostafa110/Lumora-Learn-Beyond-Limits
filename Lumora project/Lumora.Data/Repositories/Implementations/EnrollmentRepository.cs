using Lumora.Data;
using Lumora.DTOs.Dashboard;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumora.Repositories.Implementations
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsStudentEnrolledAsync(string studentId, string courseId)
        {
            return await _dbSet.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsWithProgressAsync(string studentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(e => e.LessonProgresses)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentWithLessonsProgressAsync(string studentId, string courseId)
        {
            return await _dbSet
                .Include(e => e.Course)
                    .ThenInclude(c => c.Sections)
                        .ThenInclude(s => s.Lessons)
                .Include(e => e.LessonProgresses)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        // ── Dashboard queries ──────────────────────────────────────────────

        public async Task<List<RecentEnrollmentDto>> GetRecentEnrollmentsAsync(int take = 10)
        {
            return await _dbSet.AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrolledAt)
                .Take(take)
                .Select(e => new RecentEnrollmentDto
                {
                    StudentName = e.Student.FullName,
                    CourseTitle = e.Course.Title,
                    EnrolledAt  = e.EnrolledAt
                })
                .ToListAsync();
        }
    }
}
