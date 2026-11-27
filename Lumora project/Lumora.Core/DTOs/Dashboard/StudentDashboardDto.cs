using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.DTOs.Dashboard
{
    public class StudentDashboardDto
    {
        public string StudentName { get; set; } = string.Empty;

        // KPIs
        public int EnrolledCount { get; set; }
        public int InProgressCount { get; set; }
        public int OverallCompletionPercent { get; set; }
        public int LessonsCompleted { get; set; }
        public int CertificatesCount { get; set; }

        public ContinueLearningDto? ContinueLearning { get; set; }
        public List<EnrolledCourseDto> Courses { get; set; } = new();
        public List<StudentQuizDto> Quizzes { get; set; } = new();

        public string Greeting
        {
            get
            {
                var hour = DateTime.Now.Hour;
                return hour < 12 ? "Good Morning"
                     : hour < 18 ? "Good Afternoon"
                     : "Good Evening";
            }
        }
    }

    public class ContinueLearningDto
    {
        public string CourseId { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public string? NextLessonId { get; set; }
        public string? NextLessonTitle { get; set; }
        public int ProgressPercent { get; set; }
    }

    public class EnrolledCourseDto
    {
        public string CourseId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int ProgressPercent =>
            TotalLessons == 0 ? 0 : (int)Math.Round(CompletedLessons * 100.0 / TotalLessons);
        public bool IsCompleted => TotalLessons > 0 && CompletedLessons >= TotalLessons;
    }

    public class StudentQuizDto
    {
        public string QuizId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public decimal PassingScore { get; set; }
    }
}
