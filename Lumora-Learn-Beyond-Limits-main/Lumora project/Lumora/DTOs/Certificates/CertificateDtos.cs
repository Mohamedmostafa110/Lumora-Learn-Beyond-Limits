namespace Lumora.DTOs.Certificates
{
    public class CertificateVerificationResultDto
    {
        public bool IsValid { get; set; }
        public string? VerificationId { get; set; }
        public string? StudentName { get; set; }
        public string? CourseTitle { get; set; }
        public string? InstructorName { get; set; }
        public DateTime? IssuedAt { get; set; }
        public string? PdfUrl { get; set; }
        public string? Message { get; set; }
    }
}
