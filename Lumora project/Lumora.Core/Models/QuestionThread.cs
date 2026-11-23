using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class QuestionThread
    {
        public string QuestionThreadId { get; set; }
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

}
