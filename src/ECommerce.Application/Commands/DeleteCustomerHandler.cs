using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Commands
{
    public sealed class DeleteCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<Guid>> HandleAsync(DeleteCustomerCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            if (cmd.Id == Guid.Empty)
            {
                return Result<Guid>.Failure("Customer id cannot be empty");
            }
            var customer = await _customerRepository.GetByIdAsync(cmd.Id, ct);

            if (customer is null)
            {
                return Result<Guid>.NotFound("Customer not found");
            }

            customer.SoftDelete();
            await _customerRepository.UpdateAsync(customer, ct);

            return Result<Guid>.Success(customer.Id);
        }
    }
}