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
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
