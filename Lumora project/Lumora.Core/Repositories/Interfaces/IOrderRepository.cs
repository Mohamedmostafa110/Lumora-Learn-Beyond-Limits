using Lumora.DTOs.Dashboard;
using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(string orderId);
        Task<IReadOnlyList<Order>> GetOrdersByStudentAsync(string studentId);

        // Dashboard queries
        Task<List<RecentOrderDto>> GetRecentOrdersAsync(int take = 10);
        Task<decimal>              GetTotalRevenueAsync();
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months = 6);
    }
}
