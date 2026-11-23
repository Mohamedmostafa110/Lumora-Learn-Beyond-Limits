using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string VerificationId { get; set; } = null!;
        public string PdfReference { get; set; } = null!;
        public DateTime IssuedAt { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }

}
