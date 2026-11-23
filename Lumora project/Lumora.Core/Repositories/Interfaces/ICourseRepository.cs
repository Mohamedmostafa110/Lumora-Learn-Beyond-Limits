using Lumora.DTOs.Dashboard;
using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IReadOnlyList<Course>> GetPublishedCoursesAsync(string? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null);
        Task<Course?> GetCourseWithFullCurriculumAsync(string courseId);
        Task<IReadOnlyList<Course>> GetCoursesByInstructorAsync(string instructorId);
        Task<IReadOnlyList<Course>> GetPendingApprovalCoursesAsync();
        Task<Course?> GetCourseWithReviewsAsync(string courseId);

        // Dashboard queries
        Task<List<TopCourseDto>> GetTopCoursesByEnrollmentAsync(int take = 5);
    }
}
