using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Answer
    {
        public string AnswerId { get; set; }
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string QuestionThreadId { get; set; }
        public QuestionThread QuestionThread { get; set; } = null!;
        public string InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;
    }
}
