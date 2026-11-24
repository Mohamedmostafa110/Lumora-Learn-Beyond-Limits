namespace Lumora.DTOs.Quizzes
{
    public class QuizSubmissionDto
    {
        public string QuizId { get; set; }
        public int StudentId { get; set; }
        public Dictionary<int, string> Answers { get; set; } = new(); // QuestionId -> Response
    }

    public class QuizEvaluationResultDto
    {
        public string QuizAttemptId { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public bool IsPassed { get; set; }
        public List<QuestionFeedbackDto> Feedback { get; set; } = new();
    }

    public class QuestionFeedbackDto
    {
        public string QuestionId { get; set; }
        public string Prompt { get; set; } = string.Empty;
        public string UserResponse { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
    }
}
