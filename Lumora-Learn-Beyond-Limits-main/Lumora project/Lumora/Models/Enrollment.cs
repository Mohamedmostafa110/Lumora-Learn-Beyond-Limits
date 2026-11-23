using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    }

}
