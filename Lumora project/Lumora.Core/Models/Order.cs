using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EGP";
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<OrderCoupon> OrderCoupons { get; set; } = new List<OrderCoupon>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
