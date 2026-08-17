using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
        Task AddAsync(Cart cart, CancellationToken ct = default);
        Task UpdateAsync(Cart cart, CancellationToken ct = default);
    }
}