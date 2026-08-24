using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default);
        Task AddAsync(Order order, CancellationToken ct = default);
        Task UpdateAsync(Order order, CancellationToken ct = default);
    }
}