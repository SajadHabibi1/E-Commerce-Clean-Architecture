using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceDbContext _context;

        public CartRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cart cart, CancellationToken ct = default)
        {
            await _context.Carts.AddAsync(cart,ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        {
            return await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, ct);
        }

        public async Task UpdateAsync(Cart cart, CancellationToken ct = default)
        {
            foreach (var item in cart.CartItems)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            
            await _context.SaveChangesAsync(ct);
        }
    }
}