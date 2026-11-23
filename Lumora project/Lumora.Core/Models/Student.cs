using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AccountStatus AccountStatus { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ReviewVote> ReviewVotes { get; set; } = new List<ReviewVote>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<QuestionThread> QuestionThreads { get; set; } = new List<QuestionThread>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public DateTime CreatedAt { get; set; }
    }
}
