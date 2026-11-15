using Lumora.Models;
using Lumora.Repositories.Interfaces;

namespace Lumora.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IUnitOfWork uow)
        {
            // إذا كان الكورس موجوداً مسبقاً، فالبيانات مكتملة
            if (await uow.Courses.CountAsync() > 0)
            {
                return;
            }

            // 1. الأقسام (Categories)
            var catDev = await uow.Categories.FirstOrDefaultAsync(c => c.Name == "Web Development")
                         ?? new Category { Name = "Web Development" };
            var catCloud = await uow.Categories.FirstOrDefaultAsync(c => c.Name == "Cloud & DevOps")
                           ?? new Category { Name = "Cloud & DevOps" };
            var catAI = await uow.Categories.FirstOrDefaultAsync(c => c.Name == "AI & Data Science")
                        ?? new Category { Name = "AI & Data Science" };

            if (catDev.CategoryId == 0) await uow.Categories.AddAsync(catDev);
            if (catCloud.CategoryId == 0) await uow.Categories.AddAsync(catCloud);
            if (catAI.CategoryId == 0) await uow.Categories.AddAsync(catAI);

            // 2. الوسوم (Tags)
            var tagDotNet = await uow.Tags.FirstOrDefaultAsync(t => t.Name == ".NET 10")
                            ?? new Tag { Name = ".NET 10" };
            var tagArch = await uow.Tags.FirstOrDefaultAsync(t => t.Name == "Clean Architecture")
                          ?? new Tag { Name = "Clean Architecture" };

            if (tagDotNet.TagId == 0) await uow.Tags.AddAsync(tagDotNet);
            if (tagArch.TagId == 0) await uow.Tags.AddAsync(tagArch);

            // 3. المحاضر (Instructor)
            var instructor = await uow.Instructors.FirstOrDefaultAsync(i => i.Email == "m.adel@lumora.com");
            if (instructor == null)
            {
                instructor = new Instructor
                {
                    FullName = "Eng. Mohamed Adel",
                    Email = "m.adel@lumora.com",
                    AccountStatus = AccountStatus.Active,
                    EmailVerifiedAt = DateTime.UtcNow
                };
                await uow.Instructors.AddAsync(instructor);
            }

            // 4. الطالب (Student)
            var student = await uow.Students.FirstOrDefaultAsync(s => s.Email == "ahmed.hassan@example.com");
            if (student == null)
            {
                student = new Student
                {
                    FullName = "Ahmed Hassan",
                    Email = "ahmed.hassan@example.com",
                    AccountStatus = AccountStatus.Active,
                    EmailVerifiedAt = DateTime.UtcNow
                };
                await uow.Students.AddAsync(student);
            }

            // 5. المشرف (Admin)
            var admin = await uow.Admins.FirstOrDefaultAsync(a => a.Email == "admin@lumora.com");
            if (admin == null)
            {
                admin = new Admin
                {
                    FullName = "Lumora System Administrator",
                    Email = "admin@lumora.com",
                    AccountStatus = AccountStatus.Active,
                    EmailVerifiedAt = DateTime.UtcNow
                };
                await uow.Admins.AddAsync(admin);
            }

            await uow.CompleteAsync();

            // 6. إضافة كورس منشور (Course)
            var course = new Course
            {
                Title = "Mastering .NET 10 & Microservices Architecture",
                Description = "A complete comprehensive guide to modern enterprise backend engineering with .NET 10 and Azure.",
                Difficulty = Difficulty.Advanced,
                Price = 1200.00m,
                Discount = 200.00m,
                Status = CourseStatus.Published,
                CreatedAt = DateTime.UtcNow,
                PublishedAt = DateTime.UtcNow,
                InstructorId = instructor.InstructorId,
                CourseCategories = new List<CourseCategory>
                {
                    new CourseCategory { CategoryId = catDev.CategoryId },
                    new CourseCategory { CategoryId = catCloud.CategoryId }
                },
                CourseTags = new List<CourseTag>
                {
                    new CourseTag { TagId = tagDotNet.TagId },
                    new CourseTag { TagId = tagArch.TagId }
                }
            };
            await uow.Courses.AddAsync(course);
            await uow.CompleteAsync(); // توليد CourseId

            // 7. إضافة السكاشن والدروس مع ربط CourseId و SectionId بدقة
            var section1 = new Section
            {
                Title = "Module 1: Architecture & Project Setup",
                SortOrder = 1,
                CourseId = course.CourseId
            };
            var section2 = new Section
            {
                Title = "Module 2: Advanced Entity Framework Core 10",
                SortOrder = 2,
                CourseId = course.CourseId
            };
            await uow.Repository<Section>().AddRangeAsync(new[] { section1, section2 });
            await uow.CompleteAsync(); // توليد SectionId

            var lessons = new[]
            {
                new Lesson { Title = "Lesson 1: Welcome to Lumora Platform", SortOrder = 1, CourseId = course.CourseId, SectionId = section1.SectionId },
                new Lesson { Title = "Lesson 2: N-Tier & Clean Architecture Patterns", SortOrder = 2, CourseId = course.CourseId, SectionId = section1.SectionId },
                new Lesson { Title = "Lesson 3: Fluent API & Value Conversions", SortOrder = 1, CourseId = course.CourseId, SectionId = section2.SectionId },
                new Lesson { Title = "Lesson 4: Unit of Work & Repository Pattern", SortOrder = 2, CourseId = course.CourseId, SectionId = section2.SectionId }
            };
            await uow.Repository<Lesson>().AddRangeAsync(lessons);

            // 8. إضافة اشتراك للطالب (Enrollment)
            var enrollment = new Enrollment
            {
                StudentId = student.StudentId,
                CourseId = course.CourseId,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow
            };
            await uow.Enrollments.AddAsync(enrollment);

            // 9. إضافة شهادة معتمدة بكود تحقق فريد (Certificate)
            var certificate = new Certificate
            {
                VerificationId = "LUM-2026-DOTNET10",
                PdfReference = "https://lumora.blob.core.windows.net/certificates/LUM-2026-DOTNET10.pdf",
                IssuedAt = DateTime.UtcNow,
                StudentId = student.StudentId,
                CourseId = course.CourseId
            };
            await uow.Certificates.AddAsync(certificate);

            await uow.CompleteAsync();
        }
    }
}
