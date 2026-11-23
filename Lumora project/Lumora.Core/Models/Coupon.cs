using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Coupon
    {
        public string CouponId { get; set; }
        public string Code { get; set; } = null!;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public CouponStatus Status { get; set; }
        public ICollection<OrderCoupon> OrderCoupons { get; set; } = new List<OrderCoupon>();
    }
}
