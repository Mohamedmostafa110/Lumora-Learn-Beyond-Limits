using Lumora.Data;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace Lumora.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        private ICourseRepository? _courses;
        private IEnrollmentRepository? _enrollments;
        private ICertificateRepository? _certificates;
        private IOrderRepository? _orders;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
        public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);
        public ICertificateRepository Certificates => _certificates ??= new CertificateRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

        public IGenericRepository<Student> Students => Repository<Student>();
        public IGenericRepository<Instructor> Instructors => Repository<Instructor>();
        public IGenericRepository<Admin> Admins => Repository<Admin>();
        public IGenericRepository<Category> Categories => Repository<Category>();
        public IGenericRepository<Tag> Tags => Repository<Tag>();
        public IGenericRepository<Quiz> Quizzes => Repository<Quiz>();
        public IGenericRepository<Question> Questions => Repository<Question>();
        public IGenericRepository<QuizAttempt> QuizAttempts => Repository<QuizAttempt>();
        public IGenericRepository<Payment> Payments => Repository<Payment>();
        public IGenericRepository<Coupon> Coupons => Repository<Coupon>();
        public IGenericRepository<Review> Reviews => Repository<Review>();
        public IGenericRepository<LessonProgress> LessonProgresses => Repository<LessonProgress>();
        public IGenericRepository<Note> Notes => Repository<Note>();
        public IGenericRepository<Bookmark> Bookmarks => Repository<Bookmark>();
        public IGenericRepository<QuestionThread> QuestionThreads => Repository<QuestionThread>();
        public IGenericRepository<Answer> Answers => Repository<Answer>();
        public IGenericRepository<Announcement> Announcements => Repository<Announcement>();
        public IGenericRepository<ApprovalEvent> ApprovalEvents => Repository<ApprovalEvent>();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(_context));
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
