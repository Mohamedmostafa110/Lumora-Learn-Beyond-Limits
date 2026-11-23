using System;
using System.Collections.Generic;
using System.Text;
using static Azure.Core.HttpHeader;

namespace Lumora.Models
{
    public class OrderCoupon
    {
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public decimal AppliedAmount { get; set; }
    }
}
