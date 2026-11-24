using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Note
    {
        public string NoteId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public string LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
