using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Application.Commands
{
    public sealed class UpdateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<Guid>> HandleAsync(UpdateCustomerCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            if (cmd.Id == Guid.Empty)
            {
                return Result<Guid>.Failure("Customer id cannot be empty");
            }

            try
            {
                var customer = await _customerRepository.GetByIdAsync(cmd.Id, ct);

                if (customer is null)
                {
                    return Result<Guid>.NotFound("Customer not found");
                }

                if (await _customerRepository.ExistsByEmailAsync(cmd.Email, cmd.Id, ct))
                {
                    return Result<Guid>.Failure("Email already exists");
                }

                Address? address = null;

                if (!string.IsNullOrWhiteSpace(cmd.Street) || !string.IsNullOrWhiteSpace(cmd.City) ||
                !string.IsNullOrWhiteSpace(cmd.PostalCode) ||
                !string.IsNullOrWhiteSpace(cmd.Country))
                {
                    address = new Address(cmd.Street!, cmd.City!, cmd.PostalCode!, cmd.Country!);
                }

                customer.Edit(
                    cmd.FirstName,
                    cmd.LastName,
                    cmd.Email,
                    cmd.PhoneNumber,
                    address
                );

                await _customerRepository.UpdateAsync(customer, ct);
                return Result<Guid>.Success(customer.Id);
            }

            catch (DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}