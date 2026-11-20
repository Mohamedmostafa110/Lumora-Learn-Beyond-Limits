namespace Lumora.Common.Exceptions
{
    public class InvalidCouponException : Exception
    {
        public string CouponCode { get; }

        public InvalidCouponException(string couponCode, string reason)
            : base($"Coupon '{couponCode}' is invalid: {reason}")
        {
            CouponCode = couponCode;
        }
    }

    public class CourseNotPublishedException : Exception
    {
        public int CourseId { get; }

        public CourseNotPublishedException(int courseId)
            : base($"Course #{courseId} is not in Published status and cannot be accessed.")
        {
            CourseId = courseId;
        }
    }
}
