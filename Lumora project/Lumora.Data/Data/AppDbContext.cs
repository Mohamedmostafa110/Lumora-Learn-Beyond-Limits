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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var pk = entity.FindPrimaryKey();

                if (pk is { Properties.Count: 1 } && pk.Properties[0].ClrType == typeof(string))
                {
                    var prop = pk.Properties[0];
                    prop.SetMaxLength(36);
                    prop.SetDefaultValueSql("LOWER(CONVERT(nvarchar(36), NEWID()))");
                    prop.ValueGenerated = ValueGenerated.OnAdd;
                }
            }

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreach (var p in fk.Properties.Where(p => p.ClrType == typeof(string)))
                    p.SetMaxLength(36);
            }

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                if (((IConventionForeignKey)fk).GetDeleteBehaviorConfigurationSource() != ConfigurationSource.Explicit)
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
