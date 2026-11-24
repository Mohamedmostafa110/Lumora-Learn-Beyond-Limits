using Lumora.Common.Exceptions;
using Lumora.DTOs.Certificates;
using Lumora.DTOs.Courses;
using Lumora.DTOs.Orders;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Interfaces;
using Lumora.Services.Strategies;
using Microsoft.Extensions.Logging;

namespace Lumora.Services.Implementations
{
    public class CourseCatalogService : ICourseCatalogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseCatalogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<CourseCardDto>> GetCatalogCoursesAsync(string? categoryId = null, Difficulty? difficulty = null, string? searchTerm = null)
        {
            var courses = await _unitOfWork.Courses.GetPublishedCoursesAsync(categoryId, difficulty, searchTerm);
            return courses.Select(c => new CourseCardDto
            {
                CourseId = c.CourseId,
                Title = c.Title,
                InstructorName = c.Instructor.FullName,
                Difficulty = c.Difficulty,
                Price = c.Price,
                Discount = c.Discount,
                AverageRating = c.Reviews.Count > 0 ? c.Reviews.Average(r => r.Rating) : 5.0,
                TotalReviews = c.Reviews.Count,
                Categories = c.CourseCategories.Select(cc => cc.Category.Name).ToList(),
                Tags = c.CourseTags.Select(ct => ct.Tag.Name).ToList()
            }).ToList();
        }

        public async Task<CourseDetailDto?> GetCourseDetailsAsync(string courseId)
        {
            var course = await _unitOfWork.Courses.GetCourseWithFullCurriculumAsync(courseId);
            if (course == null || course.Status != CourseStatus.Published)
            {
                return null;
            }

            return new CourseDetailDto
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorName = course.Instructor.FullName,
                Difficulty = course.Difficulty,
                Price = course.Price,
                Discount = course.Discount,
                CreatedAt = course.CreatedAt,
                Sections = course.Sections.Select(s => new SectionDetailDto
                {
                    SectionId = s.SectionId,
                    Title = s.Title,
                    SortOrder = s.SortOrder,
                    Lessons = s.Lessons.Select(l => new LessonSummaryDto
                    {
                        LessonId = l.LessonId,
                        Title = l.Title,
                        SortOrder = l.SortOrder
                    }).ToList()
                }).ToList()
            };
        }
    }

    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountStrategyFactory _discountFactory;
        private readonly ILogger<CheckoutService> _logger;

        public CheckoutService(IUnitOfWork unitOfWork, IDiscountStrategyFactory discountFactory, ILogger<CheckoutService> logger)
        {
            _unitOfWork = unitOfWork;
            _discountFactory = discountFactory;
            _logger = logger;
        }

        public async Task<OrderSummaryDto> ProcessCheckoutAsync(CheckoutRequestDto request)
        {
            // 1. Rule: Prevent duplicate purchase
            foreach (var courseId in request.CourseIds)
            {
                if (await _unitOfWork.Enrollments.IsStudentEnrolledAsync(request.StudentId, courseId))
                {
                    throw new DuplicatePurchaseException(request.StudentId, courseId);
                }
            }

            // 2. Fetch courses & compute subtotal
            var courses = new List<Course>();
            decimal subTotal = 0;
            foreach (var cid in request.CourseIds)
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(cid);
                if (course == null) continue;
                courses.Add(course);
                subTotal += (course.Price - course.Discount);
            }

            // 3. Apply Coupon Discount via Strategy Pattern if provided
            decimal discountAmount = 0;
            Coupon? coupon = null;
            if (!string.IsNullOrWhiteSpace(request.CouponCode))
            {
                coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == request.CouponCode && c.Status == CouponStatus.Active);
                if (coupon == null || DateTime.UtcNow < coupon.ValidFrom || DateTime.UtcNow > coupon.ValidTo)
                {
                    throw new InvalidCouponException(request.CouponCode, "Coupon is expired or does not exist.");
                }

                var strategy = _discountFactory.GetStrategy(coupon.DiscountType);
                discountAmount = strategy.CalculateDiscount(subTotal, coupon.DiscountValue);
            }

            var finalTotal = Math.Max(0, subTotal - discountAmount);

            // 4. Atomic transaction using Unit of Work (Order + Payment + Enrollment)
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    StudentId = request.StudentId,
                    TotalAmount = finalTotal,
                    Currency = "EGP",
                    Status = OrderStatus.Paid,
                    CreatedAt = DateTime.UtcNow
                };

                foreach (var c in courses)
                {
                    order.Items.Add(new OrderItem
                    {
                        CourseId = c.CourseId,
                        PriceAtPurchase = c.Price - c.Discount
                    });
                }

                if (coupon != null && discountAmount > 0)
                {
                    order.OrderCoupons.Add(new OrderCoupon
                    {
                        CouponId = coupon.CouponId,
                        AppliedAmount = discountAmount
                    });
                }

                order.Payments.Add(new Payment
                {
                    ExternalId = $"pay_test_{Guid.NewGuid():N}",
                    Amount = finalTotal,
                    Currency = "EGP",
                    Status = PaymentStatus.Succeeded,
                    PaidAt = DateTime.UtcNow,
                    Provider = request.PaymentProvider
                });

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CompleteAsync();

                // Enroll student into courses
                foreach (var c in courses)
                {
                    await _unitOfWork.Enrollments.AddAsync(new Enrollment
                    {
                        StudentId = request.StudentId,
                        CourseId = c.CourseId,
                        Status = EnrollmentStatus.Active,
                        EnrolledAt = DateTime.UtcNow
                    });
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Order #{OrderId} processed atomically with total: {Total}", order.OrderId, finalTotal);

                return new OrderSummaryDto
                {
                    OrderId = order.OrderId,
                    SubTotal = subTotal,
                    DiscountAmount = discountAmount,
                    TotalAmount = finalTotal,
                    Currency = "EGP",
                    Status = OrderStatus.Paid,
                    CreatedAt = order.CreatedAt,
                    CourseTitles = courses.Select(c => c.Title).ToList()
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public class CertificateVerificationService : ICertificateVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CertificateVerificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CertificateVerificationResultDto> VerifyCertificateAsync(string verificationId)
        {
            if (string.IsNullOrWhiteSpace(verificationId))
            {
                return new CertificateVerificationResultDto
                {
                    IsValid = false,
                    Message = "Please enter a valid certificate code."
                };
            }

            var certificate = await _unitOfWork.Certificates.GetByVerificationIdAsync(verificationId.Trim());
            if (certificate == null)
            {
                return new CertificateVerificationResultDto
                {
                    IsValid = false,
                    Message = $"No certificate matches verification code '{verificationId}'."
                };
            }

            return new CertificateVerificationResultDto
            {
                IsValid = true,
                VerificationId = certificate.VerificationId,
                StudentName = certificate.Student.FullName,
                CourseTitle = certificate.Course.Title,
                InstructorName = certificate.Course.Instructor.FullName,
                IssuedAt = certificate.IssuedAt,
                PdfUrl = certificate.PdfReference,
                Message = "Certificate is authentic, verified, and officially issued by Lumora."
            };
        }
    }

    public class AdminApprovalService : IAdminApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminApprovalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<CourseCardDto>> GetPendingApprovalCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.GetPendingApprovalCoursesAsync();
            return courses.Select(c => new CourseCardDto
            {
                CourseId = c.CourseId,
                Title = c.Title,
                InstructorName = c.Instructor.FullName,
                Difficulty = c.Difficulty,
                Price = c.Price
            }).ToList();
        }

        public async Task ApproveCourseAsync(string adminId, string courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null) return;

            course.Status = CourseStatus.Published;
            course.PublishedAt = DateTime.UtcNow;

            await _unitOfWork.ApprovalEvents.AddAsync(new ApprovalEvent
            {
                AdminId = adminId,
                CourseId = courseId,
                Decision = ApprovalDecision.Approved,
                ReviewedAt = DateTime.UtcNow
            });

            await _unitOfWork.CompleteAsync();
        }

        public async Task RejectCourseAsync(string adminId, string courseId, string feedback)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null) return;

            course.Status = CourseStatus.Draft;

            await _unitOfWork.ApprovalEvents.AddAsync(new ApprovalEvent
            {
                AdminId = adminId,
                CourseId = courseId,
                Decision = ApprovalDecision.Rejected,
                ReviewedAt = DateTime.UtcNow
            });

            await _unitOfWork.CompleteAsync();
        }
    }
}
