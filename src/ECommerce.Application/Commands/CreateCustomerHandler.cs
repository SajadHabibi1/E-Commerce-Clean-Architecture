using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;


namespace ECommerce.Application.Commands
{
    public sealed class CreateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<Guid>> HandleAsync(CreateCustomerCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            if (await _customerRepository.ExistsByEmailAsync(cmd.Email, ct: ct))
            {
                return Result<Guid>.Failure("Email already exists");
            }

            try
            {
                Address? address = null;

                if (!string.IsNullOrWhiteSpace(cmd.Street) || !string.IsNullOrWhiteSpace(cmd.City) ||
                !string.IsNullOrWhiteSpace(cmd.PostalCode) ||
                !string.IsNullOrWhiteSpace(cmd.Country))
                {
                    address = new Address(cmd.Street!, cmd.City!, cmd.PostalCode!, cmd.Country!);
                }

                var customer = new Customer(
                    cmd.FirstName,
                    cmd.LastName,
                    cmd.Email,
                    cmd.PhoneNumber,
                    address
                );

                await _customerRepository.AddAsync(customer, ct);
                return Result<Guid>.Success(customer.Id);
            }

            catch (DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}