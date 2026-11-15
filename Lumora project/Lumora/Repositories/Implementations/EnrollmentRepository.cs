using Lumora.Data;
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

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _dbSet.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsWithProgressAsync(int studentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(e => e.LessonProgresses)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentWithLessonsProgressAsync(int studentId, int courseId)
        {
            return await _dbSet
                .Include(e => e.Course)
                    .ThenInclude(c => c.Sections)
                        .ThenInclude(s => s.Lessons)
                .Include(e => e.LessonProgresses)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
