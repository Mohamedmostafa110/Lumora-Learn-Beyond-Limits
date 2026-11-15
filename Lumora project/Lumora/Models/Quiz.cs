using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Lumora.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = null!;
        public decimal PassingScore { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public int SectionId { get; set; }
        public Section Section { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }

}
