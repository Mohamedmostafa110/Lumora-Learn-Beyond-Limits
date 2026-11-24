using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Certificate
    {
        public string CertificateId { get; set; }
        public string VerificationId { get; set; } = null!;
        public string PdfReference { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }

}
