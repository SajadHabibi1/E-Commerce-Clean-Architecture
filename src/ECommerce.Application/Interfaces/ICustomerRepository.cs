using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Customer customer, CancellationToken ct = default);
        Task UpdateAsync(Customer customer, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = default, CancellationToken ct = default);
    }
}