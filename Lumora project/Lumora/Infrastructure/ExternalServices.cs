namespace Lumora.Infrastructure.Payments
{
    public interface IPaymentGateway
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string orderId);
        Task<bool> VerifyPaymentWebhookSignatureAsync(string payload, string signature);
    }

    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly ILogger<StripePaymentGateway> _logger;

        public StripePaymentGateway(ILogger<StripePaymentGateway> logger)
        {
            _logger = logger;
        }

        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string orderId)
        {
            _logger.LogInformation("Creating Stripe test payment intent for Order #{OrderId}, Amount: {Amount} {Currency}", orderId, amount, currency);
            await Task.Delay(50);
            return $"pi_test_{Guid.NewGuid():N}";
        }

        public async Task<bool> VerifyPaymentWebhookSignatureAsync(string payload, string signature)
        {
            await Task.Delay(10);
            return true;
        }
    }
}

namespace Lumora.Infrastructure.Pdf
{
    public interface IPdfCertificateGenerator
    {
        Task<byte[]> GenerateCertificatePdfAsync(string verificationId, string studentName, string courseTitle, string instructorName, DateTime issuedDate);
    }

    public class PdfCertificateGenerator : IPdfCertificateGenerator
    {
        private readonly ILogger<PdfCertificateGenerator> _logger;

        public PdfCertificateGenerator(ILogger<PdfCertificateGenerator> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> GenerateCertificatePdfAsync(string verificationId, string studentName, string courseTitle, string instructorName, DateTime issuedDate)
        {
            _logger.LogInformation("Generating verifiable PDF certificate {VerificationId} for {StudentName}", verificationId, studentName);
            await Task.Delay(50);
            // Returns placeholder PDF bytes
            return System.Text.Encoding.UTF8.GetBytes($"%PDF-1.4 Mock Certificate for {studentName} ({verificationId})");
        }
    }
}
