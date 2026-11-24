using Lumora.Data;
using Lumora.DTOs.Dashboard;
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

        public async Task<Order?> GetOrderWithDetailsAsync(string orderId)
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

        public async Task<IReadOnlyList<Order>> GetOrdersByStudentAsync(string studentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.StudentId == studentId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Course)
                .Include(o => o.Payments)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        // ── Dashboard queries ──────────────────────────────────────────────

        public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(int take = 10)
        {
            return await _dbSet.AsNoTracking()
                .Include(o => o.Student)
                .OrderByDescending(o => o.CreatedAt)
                .Take(take)
                .Select(o => new RecentOrderDto
                {
                    OrderId     = o.OrderId,
                    StudentName = o.Student.FullName,
                    TotalAmount = o.TotalAmount,
                    Currency    = o.Currency,
                    Status      = o.Status.ToString(),
                    CreatedAt   = o.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _dbSet
                .Where(o => o.Status == OrderStatus.Paid)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months = 6)
        {
            var since = DateTime.UtcNow.AddMonths(-months);
            return await _dbSet.AsNoTracking()
                .Where(o => o.Status == OrderStatus.Paid && o.CreatedAt >= since)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Year       = g.Key.Year,
                    Month      = g.Key.Month,
                    Revenue    = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()
                })
                .ToListAsync();
        }
    }
}
