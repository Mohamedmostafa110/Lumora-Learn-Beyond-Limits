using System;
using System.Collections.Generic;
using System.Text;
namespace Lumora.Models
{
    public class OrderCoupon
    {
        public string CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public string OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public decimal AppliedAmount { get; set; }
    }
}
