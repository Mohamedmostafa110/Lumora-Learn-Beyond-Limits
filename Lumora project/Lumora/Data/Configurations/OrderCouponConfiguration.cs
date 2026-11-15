using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class OrderCouponConfiguration : IEntityTypeConfiguration<OrderCoupon>
{
    public void Configure(EntityTypeBuilder<OrderCoupon> builder)
    {
        builder.HasKey(x => new { x.CouponId, x.OrderId });
        builder.Property(x => x.AppliedAmount).HasPrecision(18, 2);

        builder.HasOne(x => x.Order).WithMany(o => o.OrderCoupons)
            .HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Coupon).WithMany(c => c.OrderCoupons)
            .HasForeignKey(x => x.CouponId);
    }
}
