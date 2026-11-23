using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Lumora.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public CourseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
        public ICollection<ApprovalEvent> ApprovalEvents { get; set; } = new List<ApprovalEvent>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<QuestionThread> QuestionThreads { get; set; } = new List<QuestionThread>();
    }
}
