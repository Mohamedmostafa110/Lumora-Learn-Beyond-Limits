using Lumora.DTOs.Certificates;
using Lumora.DTOs.Courses;
using Lumora.DTOs.Orders;
using Lumora.DTOs.Quizzes;

namespace Lumora.Services.Interfaces
{
    public interface ICourseCatalogService
    {
        Task<IReadOnlyList<CourseCardDto>> GetCatalogCoursesAsync(int? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null);
        Task<CourseDetailDto?> GetCourseDetailsAsync(int courseId);
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
        Task<int> CreateCourseDraftAsync(int instructorId, string title, string? description, decimal price, Difficulty difficulty);
        Task SubmitCourseForApprovalAsync(int courseId, int instructorId);
        Task<IReadOnlyList<CourseCardDto>> GetInstructorCoursesAsync(int instructorId);
    }

    public interface IAdminApprovalService
    {
        Task<IReadOnlyList<CourseCardDto>> GetPendingApprovalCoursesAsync();
        Task ApproveCourseAsync(int adminId, int courseId);
        Task RejectCourseAsync(int adminId, int courseId, string feedback);
    }
}
