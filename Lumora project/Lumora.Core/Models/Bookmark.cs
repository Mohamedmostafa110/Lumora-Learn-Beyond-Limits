using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Bookmark
    {
        public string BookmarkId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Position { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public string LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }

}
