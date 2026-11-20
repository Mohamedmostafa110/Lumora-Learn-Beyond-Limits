using Lumora.Data;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumora.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet.AsNoTracking()
                .Include(o => o.Student)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Course)
                .Include(o => o.OrderCoupons)
                    .ThenInclude(oc => oc.Coupon)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByStudentAsync(int studentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.StudentId == studentId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Course)
                .Include(o => o.Payments)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
