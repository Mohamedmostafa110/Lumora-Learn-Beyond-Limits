using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Enrollment
    {
        public string EnrollmentId { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    }

}
