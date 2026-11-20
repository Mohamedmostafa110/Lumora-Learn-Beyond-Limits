using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Section
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = null!;
        public int SortOrder { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
