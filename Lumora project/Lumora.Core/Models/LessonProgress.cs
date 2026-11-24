using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class LessonProgress
    {
        public string LessonProgressId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public string LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
