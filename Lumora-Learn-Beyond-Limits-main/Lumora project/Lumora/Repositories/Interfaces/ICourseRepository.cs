using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IReadOnlyList<Course>> GetPublishedCoursesAsync(int? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null);
        Task<Course?> GetCourseWithFullCurriculumAsync(int courseId);
        Task<IReadOnlyList<Course>> GetCoursesByInstructorAsync(int instructorId);
        Task<IReadOnlyList<Course>> GetPendingApprovalCoursesAsync();
        Task<Course?> GetCourseWithReviewsAsync(int courseId);
    }
}
