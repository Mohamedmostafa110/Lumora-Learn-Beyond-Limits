namespace Lumora.DTOs.Orders
{
    public class CheckoutRequestDto
    {
        public int StudentId { get; set; }
        public List<int> CourseIds { get; set; } = new();
        public string? CouponCode { get; set; }
        public string PaymentProvider { get; set; } = "Stripe";
    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EGP";
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> CourseTitles { get; set; } = new();
    }
}
