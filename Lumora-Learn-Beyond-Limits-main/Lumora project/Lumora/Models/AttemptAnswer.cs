using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class AttemptAnswer
    {
        public int AttemptAnswerId { get; set; }
        public string Response { get; set; } = null!;
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; } = null!;
    }
}
