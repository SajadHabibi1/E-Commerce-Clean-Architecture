using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public static class CustomerMappingExtensions
    {
        public static CustomerDto ToDto(this Customer customer)
        {
            return new CustomerDto(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                customer.Address?.Street,
                customer.Address?.City,
                customer.Address?.PostalCode,
                customer.Address?.Country,
                customer.IsActive
            );
        }
    }
}