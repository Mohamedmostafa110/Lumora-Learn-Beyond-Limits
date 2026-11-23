namespace Lumora.DTOs.Courses
{
    public class CourseCardDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice => Math.Max(0, Price - Discount);
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<string> Categories { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }

    public class CourseDetailDto : CourseCardDto
    {
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SectionDetailDto> Sections { get; set; } = new();
    }

    public class SectionDetailDto
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public List<LessonSummaryDto> Lessons { get; set; } = new();
    }

    public class LessonSummaryDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsCompleted { get; set; }
    }
}
