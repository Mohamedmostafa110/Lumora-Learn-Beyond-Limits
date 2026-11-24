using Lumora.Models;
using Lumora.Models.Enums;
using Lumora.Repositories.Interfaces;

namespace Lumora.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IUnitOfWork uow)
        {
            // لو فيه كورسات يبقى الـ seed اتعمل قبل كده
            if (await uow.Courses.CountAsync() > 0)
                return;

            var now = DateTime.UtcNow;

            // ══════════════════════════════════════
            // 1. CATEGORIES
            // ══════════════════════════════════════
            var catDev    = new Category { Name = "Web Development" };
            var catCloud  = new Category { Name = "Cloud & DevOps" };
            var catData   = new Category { Name = "Data Science & AI" };
            var catMobile = new Category { Name = "Mobile Development" };

            // ══════════════════════════════════════
            // 2. TAGS
            // ══════════════════════════════════════
            var tagDotNet  = new Tag { Name = ".NET 10" };
            var tagArch    = new Tag { Name = "Clean Architecture" };
            var tagDocker  = new Tag { Name = "Docker" };
            var tagAzure   = new Tag { Name = "Azure" };
            var tagReact   = new Tag { Name = "React" };
            var tagPython  = new Tag { Name = "Python" };
            var tagML      = new Tag { Name = "Machine Learning" };

            // ══════════════════════════════════════
            // 3. USERS  (1 Admin · 3 Instructors · 4 Students)
            // ══════════════════════════════════════
            var admin = new Admin
            {
                FullName          = "Lumora System Administrator",
                Email             = "admin@lumora.com",
                AccountStatus     = AccountStatus.Active,
                EmailVerifiedAt   = now.AddDays(-90)
            };

            var instructor1 = new Instructor
            {
                FullName          = "Eng. Mohamed Adel",
                Email             = "m.adel@lumora.com",
                AccountStatus     = AccountStatus.Active,
                EmailVerifiedAt   = now.AddDays(-80)
            };
            var instructor2 = new Instructor
            {
                FullName          = "Dr. Sara Khaled",
                Email             = "s.khaled@lumora.com",
                AccountStatus     = AccountStatus.Active,
                EmailVerifiedAt   = now.AddDays(-60)
            };
            var instructor3 = new Instructor
            {
                FullName          = "Omar Fathy",
                Email             = "o.fathy@lumora.com",
                AccountStatus     = AccountStatus.Active,
                EmailVerifiedAt   = now.AddDays(-45)
            };

            var student1 = new Student { FullName = "Ahmed Hassan",    Email = "ahmed.hassan@example.com",  AccountStatus = AccountStatus.Active, EmailVerifiedAt = now.AddDays(-70), CreatedAt = now.AddDays(-70) };
            var student2 = new Student { FullName = "Nour El-Din",     Email = "nour.eldin@example.com",    AccountStatus = AccountStatus.Active, EmailVerifiedAt = now.AddDays(-50), CreatedAt = now.AddDays(-50) };
            var student3 = new Student { FullName = "Rania Mostafa",   Email = "rania.m@example.com",       AccountStatus = AccountStatus.Active, EmailVerifiedAt = now.AddDays(-40), CreatedAt = now.AddDays(-40) };
            var student4 = new Student { FullName = "Kareem Salah",    Email = "kareem.salah@example.com",  AccountStatus = AccountStatus.Active, EmailVerifiedAt = now.AddDays(-20), CreatedAt = now.AddDays(-20) };

            // ══════════════════════════════════════
            // 4. COUPONS
            // ══════════════════════════════════════
            var coupon1 = new Coupon
            {
                Code          = "LUMORA20",
                DiscountType  = DiscountType.Percentage,
                DiscountValue = 20m,
                ValidFrom     = now.AddDays(-30),
                ValidTo       = now.AddDays(60),
                Status        = CouponStatus.Active
            };
            var coupon2 = new Coupon
            {
                Code          = "FLAT100",
                DiscountType  = DiscountType.FixedAmount,
                DiscountValue = 100m,
                ValidFrom     = now.AddDays(-10),
                ValidTo       = now.AddDays(30),
                Status        = CouponStatus.Active
            };

            // ══════════════════════════════════════
            // 5. COURSE 1 — .NET Microservices  (Instructor1)
            // ══════════════════════════════════════
            var course1 = new Course
            {
                Title       = "Mastering .NET 10 & Microservices Architecture",
                Description = "A complete guide to enterprise backend with .NET 10, Docker, and Azure.",
                Difficulty  = Difficulty.Advanced,
                Price       = 1200.00m,
                Discount    = 200.00m,
                Status      = CourseStatus.Published,
                CreatedAt   = now.AddDays(-60),
                PublishedAt = now.AddDays(-55),
                Instructor  = instructor1,
                CourseCategories = new List<CourseCategory>
                {
                    new() { Category = catDev },
                    new() { Category = catCloud }
                },
                CourseTags = new List<CourseTag>
                {
                    new() { Tag = tagDotNet },
                    new() { Tag = tagArch },
                    new() { Tag = tagDocker },
                    new() { Tag = tagAzure }
                },
                ApprovalEvents = new List<ApprovalEvent>
                {
                    new() { Admin = admin, Decision = ApprovalDecision.Approved, ReviewedAt = now.AddDays(-55) }
                },
                Announcements = new List<Announcement>
                {
                    new()
                    {
                        Title       = "Welcome to the Course!",
                        Body        = "Hi everyone, excited to have you on board. We start with architecture fundamentals.",
                        PublishedAt = now.AddDays(-55),
                        Instructor  = instructor1
                    },
                    new()
                    {
                        Title       = "New Section Added: Azure Deployment",
                        Body        = "We just added a brand-new section on deploying microservices to Azure Kubernetes Service.",
                        PublishedAt = now.AddDays(-10),
                        Instructor  = instructor1
                    }
                }
            };

            // --- Sections & Lessons for Course 1 ---
            var c1s1 = new Section { Title = "Module 1: Architecture & Project Setup",          SortOrder = 1, Course = course1 };
            var c1s2 = new Section { Title = "Module 2: Advanced EF Core 10",                   SortOrder = 2, Course = course1 };
            var c1s3 = new Section { Title = "Module 3: Docker & Container Orchestration",       SortOrder = 3, Course = course1 };

            var c1s1l1 = new Lesson { Title = "Welcome & Course Overview",             SortOrder = 1, Course = course1, Section = c1s1 };
            var c1s1l2 = new Lesson { Title = "N-Tier vs Clean Architecture",          SortOrder = 2, Course = course1, Section = c1s1 };
            var c1s2l1 = new Lesson { Title = "Fluent API & Value Conversions",        SortOrder = 1, Course = course1, Section = c1s2 };
            var c1s2l2 = new Lesson { Title = "Unit of Work & Repository Pattern",     SortOrder = 2, Course = course1, Section = c1s2 };
            var c1s2l3 = new Lesson { Title = "Migrations & Seed Strategies",          SortOrder = 3, Course = course1, Section = c1s2 };
            var c1s3l1 = new Lesson { Title = "Introduction to Docker",                SortOrder = 1, Course = course1, Section = c1s3 };
            var c1s3l2 = new Lesson { Title = "Docker Compose for .NET Apps",          SortOrder = 2, Course = course1, Section = c1s3 };

            c1s1.Lessons = new List<Lesson> { c1s1l1, c1s1l2 };
            c1s2.Lessons = new List<Lesson> { c1s2l1, c1s2l2, c1s2l3 };
            c1s3.Lessons = new List<Lesson> { c1s3l1, c1s3l2 };
            course1.Sections = new List<Section> { c1s1, c1s2, c1s3 };

            // --- Enrollment: Student1 → Completed ---
            var enroll1 = new Enrollment
            {
                Student    = student1,
                Course     = course1,
                Status     = EnrollmentStatus.Completed,
                EnrolledAt = now.AddDays(-50),
                CompletedAt = now.AddDays(-5),
                LessonProgresses = new List<LessonProgress>
                {
                    new() { Lesson = c1s1l1, IsCompleted = true,  CompletedAt = now.AddDays(-48) },
                    new() { Lesson = c1s1l2, IsCompleted = true,  CompletedAt = now.AddDays(-46) },
                    new() { Lesson = c1s2l1, IsCompleted = true,  CompletedAt = now.AddDays(-40) },
                    new() { Lesson = c1s2l2, IsCompleted = true,  CompletedAt = now.AddDays(-38) },
                    new() { Lesson = c1s2l3, IsCompleted = true,  CompletedAt = now.AddDays(-30) },
                    new() { Lesson = c1s3l1, IsCompleted = true,  CompletedAt = now.AddDays(-20) },
                    new() { Lesson = c1s3l2, IsCompleted = true,  CompletedAt = now.AddDays(-5)  }
                }
            };

            // --- Enrollment: Student2 → InProgress ---
            var enroll2 = new Enrollment
            {
                Student    = student2,
                Course     = course1,
                Status     = EnrollmentStatus.Active,
                EnrolledAt = now.AddDays(-20),
                LessonProgresses = new List<LessonProgress>
                {
                    new() { Lesson = c1s1l1, IsCompleted = true,  CompletedAt = now.AddDays(-18) },
                    new() { Lesson = c1s1l2, IsCompleted = true,  CompletedAt = now.AddDays(-15) },
                    new() { Lesson = c1s2l1, IsCompleted = false }
                }
            };

            course1.Enrollments = new List<Enrollment> { enroll1, enroll2 };

            // --- Certificate for Student1 ---
            course1.Certificates = new List<Certificate>
            {
                new()
                {
                    Student        = student1,
                    VerificationId = "LUM-2026-DOTNET10",
                    PdfReference   = "certificates/LUM-2026-DOTNET10.pdf",
                    IssuedAt       = now.AddDays(-5)
                }
            };

            // --- Reviews ---
            course1.Reviews = new List<Review>
            {
                new() { Student = student1, Rating = 5, Body = "Absolutely brilliant! Best .NET course I've ever taken.",        CreatedAt = now.AddDays(-4) },
                new() { Student = student2, Rating = 4, Body = "Very detailed. The Docker section could be a bit more in-depth.", CreatedAt = now.AddDays(-10) }
            };

            // --- Q&A Thread ---
            var thread1 = new QuestionThread
            {
                Body      = "What is the difference between AddScoped and AddTransient in DI?",
                CreatedAt = now.AddDays(-25),
                Student   = student2,
                Course    = course1,
                Answers   = new List<Answer>
                {
                    new() { Body = "AddScoped creates one instance per HTTP request, while AddTransient creates a new instance every time it is requested.", CreatedAt = now.AddDays(-24), Instructor = instructor1 }
                }
            };
            course1.QuestionThreads = new List<QuestionThread> { thread1 };

            // --- Bookmarks & Notes for Student2 ---
            student2.Bookmarks = new List<Bookmark>
            {
                new() { Lesson = c1s2l1, Position = 420, CreatedAt = now.AddDays(-14) }
            };
            student2.Notes = new List<Note>
            {
                new() { Lesson = c1s2l1, Content = "Remember: HasConversion<> is crucial for enum → string mapping.", CreatedAt = now.AddDays(-14) }
            };

            // --- Orders & Payments ---
            var order1 = new Order
            {
                Student     = student1,
                TotalAmount = 1000.00m,
                Currency    = "EGP",
                Status      = OrderStatus.Paid,
                CreatedAt   = now.AddDays(-50),
                Items = new List<OrderItem>
                {
                    new() { Course = course1, PriceAtPurchase = 1000.00m }
                },
                Payments = new List<Payment>
                {
                    new()
                    {
                        ExternalId = "stripe_pi_course1_student1",
                        Amount     = 1000.00m,
                        Currency   = "EGP",
                        Status     = PaymentStatus.Succeeded,
                        PaidAt     = now.AddDays(-50),
                        Provider   = "Stripe"
                    }
                }
            };

            var order2 = new Order
            {
                Student     = student2,
                TotalAmount = 960.00m,
                Currency    = "EGP",
                Status      = OrderStatus.Paid,
                CreatedAt   = now.AddDays(-20),
                Items = new List<OrderItem>
                {
                    new() { Course = course1, PriceAtPurchase = 1000.00m }
                },
                OrderCoupons = new List<OrderCoupon>
                {
                    new() { Coupon = coupon2 }
                },
                Payments = new List<Payment>
                {
                    new()
                    {
                        ExternalId = "stripe_pi_course1_student2",
                        Amount     = 960.00m,
                        Currency   = "EGP",
                        Status     = PaymentStatus.Succeeded,
                        PaidAt     = now.AddDays(-20),
                        Provider   = "Stripe"
                    }
                }
            };

            // ══════════════════════════════════════
            // 6. COURSE 2 — Python & ML (Instructor2)
            // ══════════════════════════════════════
            var course2 = new Course
            {
                Title       = "Python for Machine Learning A-Z",
                Description = "From Python basics to production-ready ML models using scikit-learn and TensorFlow.",
                Difficulty  = Difficulty.Intermediate,
                Price       = 900.00m,
                Discount    = 0m,
                Status      = CourseStatus.Published,
                CreatedAt   = now.AddDays(-45),
                PublishedAt = now.AddDays(-40),
                Instructor  = instructor2,
                CourseCategories = new List<CourseCategory>
                {
                    new() { Category = catData }
                },
                CourseTags = new List<CourseTag>
                {
                    new() { Tag = tagPython },
                    new() { Tag = tagML }
                },
                ApprovalEvents = new List<ApprovalEvent>
                {
                    new() { Admin = admin, Decision = ApprovalDecision.Approved, ReviewedAt = now.AddDays(-40) }
                }
            };

            var c2s1 = new Section { Title = "Part 1: Python Foundations",          SortOrder = 1, Course = course2 };
            var c2s2 = new Section { Title = "Part 2: Data Wrangling with Pandas",  SortOrder = 2, Course = course2 };
            var c2s3 = new Section { Title = "Part 3: Supervised Learning",          SortOrder = 3, Course = course2 };

            var c2s1l1 = new Lesson { Title = "Python Setup & Virtual Environments", SortOrder = 1, Course = course2, Section = c2s1 };
            var c2s1l2 = new Lesson { Title = "Data Types, Lists & Dictionaries",    SortOrder = 2, Course = course2, Section = c2s1 };
            var c2s2l1 = new Lesson { Title = "DataFrames & Series",                 SortOrder = 1, Course = course2, Section = c2s2 };
            var c2s3l1 = new Lesson { Title = "Linear Regression Deep Dive",         SortOrder = 1, Course = course2, Section = c2s3 };
            var c2s3l2 = new Lesson { Title = "Decision Trees & Random Forests",     SortOrder = 2, Course = course2, Section = c2s3 };

            c2s1.Lessons = new List<Lesson> { c2s1l1, c2s1l2 };
            c2s2.Lessons = new List<Lesson> { c2s2l1 };
            c2s3.Lessons = new List<Lesson> { c2s3l1, c2s3l2 };
            course2.Sections = new List<Section> { c2s1, c2s2, c2s3 };

            // Student3 enrolled but not completed yet
            var enroll3 = new Enrollment
            {
                Student    = student3,
                Course     = course2,
                Status     = EnrollmentStatus.Active,
                EnrolledAt = now.AddDays(-15),
                LessonProgresses = new List<LessonProgress>
                {
                    new() { Lesson = c2s1l1, IsCompleted = true,  CompletedAt = now.AddDays(-13) },
                    new() { Lesson = c2s1l2, IsCompleted = true,  CompletedAt = now.AddDays(-11) },
                    new() { Lesson = c2s2l1, IsCompleted = false }
                }
            };
            course2.Enrollments = new List<Enrollment> { enroll3 };
            course2.Reviews = new List<Review>
            {
                new() { Student = student3, Rating = 5, Body = "Dr. Sara explains complex ML concepts with great clarity!", CreatedAt = now.AddDays(-5) }
            };

            var order3 = new Order
            {
                Student     = student3,
                TotalAmount = 900.00m,
                Currency    = "EGP",
                Status      = OrderStatus.Paid,
                CreatedAt   = now.AddDays(-15),
                Items = new List<OrderItem>
                {
                    new() { Course = course2, PriceAtPurchase = 900.00m }
                },
                Payments = new List<Payment>
                {
                    new()
                    {
                        ExternalId = "stripe_pi_course2_student3",
                        Amount     = 900.00m,
                        Currency   = "EGP",
                        Status     = PaymentStatus.Succeeded,
                        PaidAt     = now.AddDays(-15),
                        Provider   = "Stripe"
                    }
                }
            };

            // ══════════════════════════════════════
            // 7. COURSE 3 — React (Instructor3) — Pending Approval
            // ══════════════════════════════════════
            var course3 = new Course
            {
                Title       = "React 19 & Next.js 15: Full-Stack Mastery",
                Description = "Build blazing-fast full-stack apps with React Server Components and Next.js 15.",
                Difficulty  = Difficulty.Intermediate,
                Price       = 850.00m,
                Discount    = 50.00m,
                Status      = CourseStatus.PendingApproval,
                CreatedAt   = now.AddDays(-5),
                Instructor  = instructor3,
                CourseCategories = new List<CourseCategory>
                {
                    new() { Category = catDev },
                    new() { Category = catMobile }
                },
                CourseTags = new List<CourseTag>
                {
                    new() { Tag = tagReact }
                },
                // لا يوجد ApprovalEvent بعد — الكورس لسه في انتظار المراجعة
                ApprovalEvents = new List<ApprovalEvent>()
            };

            var c3s1 = new Section { Title = "Chapter 1: React Fundamentals Revisited", SortOrder = 1, Course = course3 };
            var c3s1l1 = new Lesson { Title = "JSX, Components & Hooks Refresher",      SortOrder = 1, Course = course3, Section = c3s1 };
            var c3s1l2 = new Lesson { Title = "React Server Components Explained",       SortOrder = 2, Course = course3, Section = c3s1 };
            c3s1.Lessons = new List<Lesson> { c3s1l1, c3s1l2 };
            course3.Sections = new List<Section> { c3s1 };

            // Student4 placed an order for course3 but it's still pending
            var order4 = new Order
            {
                Student     = student4,
                TotalAmount = 800.00m,
                Currency    = "EGP",
                Status      = OrderStatus.Pending,
                CreatedAt   = now.AddDays(-1),
                Items = new List<OrderItem>
                {
                    new() { Course = course3, PriceAtPurchase = 800.00m }
                },
                OrderCoupons = new List<OrderCoupon>
                {
                    new() { Coupon = coupon1 }
                }
            };

            // ══════════════════════════════════════
            // 8. PERSIST EVERYTHING
            // ══════════════════════════════════════
            await uow.Courses.AddAsync(course1);
            await uow.Courses.AddAsync(course2);
            await uow.Courses.AddAsync(course3);
            await uow.Orders.AddAsync(order1);
            await uow.Orders.AddAsync(order2);
            await uow.Orders.AddAsync(order3);
            await uow.Orders.AddAsync(order4);
            await uow.Coupons.AddAsync(coupon1);
            await uow.Coupons.AddAsync(coupon2);

            await uow.CompleteAsync();
        }
    }
}