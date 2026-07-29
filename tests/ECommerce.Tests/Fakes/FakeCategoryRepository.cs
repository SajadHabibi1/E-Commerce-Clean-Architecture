using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Tests.Fakes
{
    public class FakeCategoryRepository : ICategoryRepository
    {
        public List<Category> Categories { get; } = new();

        public Task AddAsync(Category category, CancellationToken ct = default)
        {
            Categories.Add(category);
            return Task.CompletedTask;
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var category = Categories.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(category);
        }

        public Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default)
        {
            return Task.FromResult<IReadOnlyList<Category>>(Categories);
        }

        public Task UpdateAsync(Category category, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByNameAsync(string name, Guid? excludeId = default, CancellationToken ct = default)
        {
            var category = Categories.Any(c => c.Name == name && (excludeId == null || c.Id != excludeId));
            return Task.FromResult(category);
        }
    }
}