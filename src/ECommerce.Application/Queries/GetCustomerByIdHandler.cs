using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Queries
{
    public sealed class GetCustomerByIdHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<CustomerDto>> HandleAsync(GetCustomerByIdQuery query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            if (query.Id == Guid.Empty)
            {
                return Result<CustomerDto>.Failure("Customer id cannot be empty");
            }

            var customer = await _customerRepository.GetByIdAsync(query.Id, ct);

            if (customer is null)
            {
                return Result<CustomerDto>.NotFound("Customer not found");
            }

            var dto = customer.ToDto();

            return Result<CustomerDto>.Success(dto);
        }
    }
}