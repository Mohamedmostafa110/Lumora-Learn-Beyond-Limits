
using Lumora.DTOs.Student;

namespace Lumora.Services.Interfaces
{
    public interface IStudentPortalService
    {
        Task<IReadOnlyList<StudentCourseDto>>
            GetMyCoursesAsync(string studentId);
    }
}
