using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsWithProgressAsync(int studentId);
        Task<Enrollment?> GetEnrollmentWithLessonsProgressAsync(int studentId, int courseId);
    }
}
