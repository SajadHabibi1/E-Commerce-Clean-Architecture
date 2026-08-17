using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Tests.Fakes
{
    public class FakeCartRepository : ICartRepository
    {
        public List<Cart> Carts { get; } = new();

        public Task AddAsync(Cart cart, CancellationToken ct = default)
        {
            Carts.Add(cart);
            return Task.CompletedTask;
        }

        public Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        {
            var cart = Carts.FirstOrDefault(c => c.CustomerId == customerId);
            return Task.FromResult(cart);
        }

        public Task UpdateAsync(Cart cart, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}