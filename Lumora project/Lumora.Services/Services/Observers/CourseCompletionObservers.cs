using Lumora.Repositories.Interfaces;

namespace Lumora.Services.Observers
{
    public interface ICourseCompletionObserver
    {
        Task OnCourseCompletedAsync(string studentId, string courseId);
    }

    public class CertificateGenerationObserver : ICourseCompletionObserver
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CertificateGenerationObserver> _logger;

        public CertificateGenerationObserver(IUnitOfWork unitOfWork, ILogger<CertificateGenerationObserver> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task OnCourseCompletedAsync(string studentId, string courseId)
        {
            _logger.LogInformation("Observer triggered: Generating verifiable certificate for Student #{StudentId} in Course #{CourseId}", studentId, courseId);

            // Check if certificate already exists
            var existing = await _unitOfWork.Certificates.FirstOrDefaultAsync(c => c.StudentId == studentId && c.CourseId == courseId);
            if (existing != null)
            {
                return;
            }

            var verificationId = $"LUM-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            var certificate = new Models.Certificate
            {
                StudentId = studentId,
                CourseId = courseId,
                VerificationId = verificationId,
                PdfReference = $"certificates/{verificationId}.pdf",
                IssuedAt = DateTime.UtcNow
            };

            await _unitOfWork.Certificates.AddAsync(certificate);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Certificate successfully created with VerificationId: {VerificationId}", verificationId);
        }
    }
}
