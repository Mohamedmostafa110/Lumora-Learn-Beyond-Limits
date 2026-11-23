using Lumora.Data;
using Lumora.DTOs.Dashboard;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumora.Repositories.Implementations
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Course>> GetPublishedCoursesAsync(
    string? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null)
        {
            var query = _dbSet.AsNoTracking()
                .Where(c => c.Status == CourseStatus.Published)
                .Include(c => c.Instructor)
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                query = query.Where(c => c.CourseCategories.Any(cc => cc.CategoryId == categoryId));
            }

            if (difficulty.HasValue)
            {
                query = query.Where(c => c.Difficulty == difficulty.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Title.Contains(searchTerm) ||
                                         (c.Description != null && c.Description.Contains(searchTerm)));
            }

            return await query.ToListAsync();
        }

        public async Task<Course?> GetCourseWithFullCurriculumAsync(string courseId)
        {
            return await _dbSet.AsNoTracking()
                .Include(c => c.Instructor)
                .Include(c => c.Sections.OrderBy(s => s.SortOrder))
                    .ThenInclude(s => s.Lessons.OrderBy(l => l.SortOrder))
                        .ThenInclude(l => l.MediaAssets)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Quizzes)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IReadOnlyList<Course>> GetCoursesByInstructorAsync(string instructorId)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Enrollments)
                .Include(c => c.Reviews)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Course>> GetPendingApprovalCoursesAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.Status == CourseStatus.PendingApproval)
                .Include(c => c.Instructor)
                .Include(c => c.Sections)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseWithReviewsAsync(string courseId)
        {
            return await _dbSet.AsNoTracking()
                .Include(c => c.Reviews)
                    .ThenInclude(r => r.Student)
                .Include(c => c.Reviews)
                    .ThenInclude(r => r.Votes)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        // ── Dashboard queries ──────────────────────────────────────────────

        public async Task<List<TopCourseDto>> GetTopCoursesByEnrollmentAsync(int take = 5)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.Status == CourseStatus.Published)
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Include(c => c.Reviews)
                .Include(c => c.OrderItems)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(take)
                .Select(c => new TopCourseDto
                {
                    CourseId        = c.CourseId,
                    Title           = c.Title,
                    InstructorName  = c.Instructor.FullName,
                    EnrollmentCount = c.Enrollments.Count,
                    AverageRating   = c.Reviews.Count > 0
                                        ? c.Reviews.Average(r => r.Rating)
                                        : 0,
                    Revenue         = c.OrderItems.Sum(i => i.PriceAtPurchase)
                })
                .ToListAsync();
        }
    }
}
