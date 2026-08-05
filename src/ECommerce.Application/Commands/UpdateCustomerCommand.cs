namespace ECommerce.Application.Commands
{
    public sealed record UpdateCustomerCommand(
        Guid Id,
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