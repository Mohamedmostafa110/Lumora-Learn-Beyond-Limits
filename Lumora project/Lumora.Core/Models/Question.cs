using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string Prompt { get; set; } = null!;
        public QuestionType Type { get; set; }
        public string? Explanation { get; set; }
        public int SortOrder { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();
    }
}
