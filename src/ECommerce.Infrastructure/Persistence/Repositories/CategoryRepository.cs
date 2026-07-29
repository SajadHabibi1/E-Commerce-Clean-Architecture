using System.Data.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ECommerceDbContext _context;

        public CategoryRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Categories
            .ToListAsync(ct);
        }

        public async Task AddAsync(Category category, CancellationToken ct = default)
        {
            await _context.Categories.AddAsync(category, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Category category, CancellationToken ct = default)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = default, CancellationToken ct = default)
        {
            return await _context.Categories
            .AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId), ct);
        }
    }
}