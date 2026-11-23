using Lumora.Data.Configurations;
using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Admin> Admins => Set<Admin>();

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();
        public DbSet<CourseTag> CourseTags => Set<CourseTag>();
        public DbSet<ApprovalEvent> ApprovalEvents => Set<ApprovalEvent>();
        public DbSet<Announcement> Announcements => Set<Announcement>();

        public DbSet<Section> Sections => Set<Section>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();

        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<AttemptAnswer> AttemptAnswers => Set<AttemptAnswer>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<OrderCoupon> OrderCoupons => Set<OrderCoupon>();
        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
        public DbSet<Certificate> Certificates => Set<Certificate>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ReviewVote> ReviewVotes => Set<ReviewVote>();
        public DbSet<QuestionThread> QuestionThreads => Set<QuestionThread>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
        public DbSet<Note> Notes => Set<Note>();

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=LumoraDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalEventConfiguration());
            modelBuilder.ApplyConfiguration(new AttemptAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CertificateConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
            modelBuilder.ApplyConfiguration(new CourseCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTagConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new LessonProgressConfiguration());
            modelBuilder.ApplyConfiguration(new MediaAssetConfiguration());
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderCouponConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionThreadConfiguration());
            modelBuilder.ApplyConfiguration(new QuizAttemptConfiguration());
            modelBuilder.ApplyConfiguration(new QuizConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewVoteConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                if (((IConventionForeignKey)fk).GetDeleteBehaviorConfigurationSource() != ConfigurationSource.Explicit)
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
