namespace ECommerce.Api.Contracts.Customers
{
    public sealed record UpdateCustomerRequest(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? Street,
        string? City,
        string? PostalCode,
        string? Country
    );
}