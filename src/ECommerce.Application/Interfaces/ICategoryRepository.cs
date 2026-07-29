using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Category category, CancellationToken ct = default);
        Task UpdateAsync(Category category, CancellationToken ct = default);
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = default, CancellationToken ct = default);
    }
}