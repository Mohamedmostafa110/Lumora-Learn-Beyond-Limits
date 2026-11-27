using Lumora.DTOs.Certificates;
using Lumora.DTOs.Courses;
using Lumora.DTOs.Dashboard;
using Lumora.DTOs.Orders;
using Lumora.DTOs.Quizzes;

namespace Lumora.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync();                           // Admin
        Task<StudentDashboardDto> GetStudentDashboardAsync(string studentId); // Student
    }

    public interface ICourseCatalogService
    {
        Task<IReadOnlyList<CourseCardDto>> GetCatalogCoursesAsync(string? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null);
        Task<CourseDetailDto?> GetCourseDetailsAsync(string courseId);
    }

    public interface ICheckoutService
    {
        Task<OrderSummaryDto> ProcessCheckoutAsync(CheckoutRequestDto request);
    }

    public interface IStudentLearningService
    {
        Task<IReadOnlyList<CourseCardDto>> GetEnrolledCoursesAsync(int studentId);
        Task<string> GetSecureLessonStreamingUrlAsync(int studentId, int lessonId);
        Task MarkLessonCompleteAsync(int studentId, int lessonId);
        Task AddLessonBookmarkAsync(int studentId, int lessonId, int positionSeconds);
        Task AddLessonNoteAsync(int studentId, int lessonId, string content);
    }

    public interface IQuizService
    {
        Task<QuizEvaluationResultDto> SubmitQuizAttemptAsync(QuizSubmissionDto submission);
    }

    public interface ICertificateVerificationService
    {
        Task<CertificateVerificationResultDto> VerifyCertificateAsync(string verificationId);
    }

    public interface IInstructorService
    {
        Task<int> CreateCourseDraftAsync(string instructorId, string title, string? description, decimal price, Difficulty difficulty);
        Task SubmitCourseForApprovalAsync(string courseId, string instructorId);
        Task<IReadOnlyList<CourseCardDto>> GetInstructorCoursesAsync(string instructorId);
    }

    public interface IAdminApprovalService
    {
        Task<IReadOnlyList<CourseCardDto>> GetPendingApprovalCoursesAsync();
        Task ApproveCourseAsync(string adminId, string courseId);
        Task RejectCourseAsync(string adminId, string courseId, string feedback);
    }
}
