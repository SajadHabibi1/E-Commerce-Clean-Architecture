using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Queries
{
    public sealed class GetAllCustomersHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<IReadOnlyList<CustomerDto>>> HandleAsync(GetAllCustomersQuery query, CancellationToken ct = default)
        {
            var customers = await _customerRepository.GetAllAsync(ct);

            var dtos = customers.Select(customer => customer.ToDto()).ToList();

            return Result<IReadOnlyList<CustomerDto>>.Success(dtos);
        }
    }
}