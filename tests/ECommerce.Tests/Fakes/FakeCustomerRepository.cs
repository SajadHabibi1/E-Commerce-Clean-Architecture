using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Tests.Fakes
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        public List<Customer> Customers { get; } = new();

        public Task AddAsync(Customer customer, CancellationToken ct = default)
        {
            Customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var customer = Customers.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(customer);
        }

        public Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default)
        {
            return Task.FromResult<IReadOnlyList<Customer>>(Customers);
        }

        public Task UpdateAsync(Customer customer, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = default, CancellationToken ct = default)
        {
            var customer = Customers.Any(c => c.Email == email && (excludeId == null || c.Id != excludeId));
            return Task.FromResult(customer);
        }
    }
}