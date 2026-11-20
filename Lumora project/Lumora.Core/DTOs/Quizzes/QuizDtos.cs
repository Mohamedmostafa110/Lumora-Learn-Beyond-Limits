namespace Lumora.DTOs.Quizzes
{
    public class QuizSubmissionDto
    {
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public Dictionary<int, string> Answers { get; set; } = new(); // QuestionId -> Response
    }

    public class QuizEvaluationResultDto
    {
        public int QuizAttemptId { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public bool IsPassed { get; set; }
        public List<QuestionFeedbackDto> Feedback { get; set; } = new();
    }

    public class QuestionFeedbackDto
    {
        public int QuestionId { get; set; }
        public string Prompt { get; set; } = string.Empty;
        public string UserResponse { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
    }
}
