using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class QuizAttempt
    {
        public string QuizAttemptId { get; set; }
        public decimal Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime AttemptedAt { get; set; }
        public string QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();
    }
}
