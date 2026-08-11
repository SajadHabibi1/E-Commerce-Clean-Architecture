using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ECommerceDbContext _context;

        public CustomerRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer, CancellationToken ct = default)
        {
            await _context.Customers.AddAsync(customer, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null, CancellationToken ct = default)
        {
            return await _context.Customers
            .AnyAsync(c => c.Email == email && (excludeId == null || c.Id != excludeId), ct);
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Customers.ToListAsync(ct);
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken ct = default)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(ct);
        }
    }
}