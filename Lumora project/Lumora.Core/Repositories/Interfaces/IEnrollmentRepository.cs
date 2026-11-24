using Lumora.DTOs.Dashboard;
using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<bool> IsStudentEnrolledAsync(string studentId, string courseId);
        Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsWithProgressAsync(string studentId);
        Task<Enrollment?> GetEnrollmentWithLessonsProgressAsync(string studentId, string courseId);

        // Dashboard queries
        Task<List<RecentEnrollmentDto>> GetRecentEnrollmentsAsync(int take = 10);
    }
}
