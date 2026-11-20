using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<IReadOnlyList<Order>> GetOrdersByStudentAsync(int studentId);
    }
}
