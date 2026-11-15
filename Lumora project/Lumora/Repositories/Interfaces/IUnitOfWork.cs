using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        ICertificateRepository Certificates { get; }
        IOrderRepository Orders { get; }

        IGenericRepository<Student> Students { get; }
        IGenericRepository<Instructor> Instructors { get; }
        IGenericRepository<Admin> Admins { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Tag> Tags { get; }
        IGenericRepository<Quiz> Quizzes { get; }
        IGenericRepository<Question> Questions { get; }
        IGenericRepository<QuizAttempt> QuizAttempts { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Coupon> Coupons { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<LessonProgress> LessonProgresses { get; }
        IGenericRepository<Note> Notes { get; }
        IGenericRepository<Bookmark> Bookmarks { get; }
        IGenericRepository<QuestionThread> QuestionThreads { get; }
        IGenericRepository<Answer> Answers { get; }
        IGenericRepository<Announcement> Announcements { get; }
        IGenericRepository<ApprovalEvent> ApprovalEvents { get; }

        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
