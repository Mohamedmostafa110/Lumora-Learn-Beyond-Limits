
namespace Lumora.DTOs.Student
{
    public class StudentCourseDto
    {
        public string EnrollmentId { get; set; } = string.Empty;

        public string CourseId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public Difficulty Difficulty { get; set; }

        public DateTime EnrolledAt { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int TotalLessons { get; set; }

        public int CompletedLessons { get; set; }

        public int ProgressPercentage =>
            TotalLessons == 0
                ? 0
                : (int)Math.Round(
                    CompletedLessons * 100.0 / TotalLessons
                  );
    }
}
