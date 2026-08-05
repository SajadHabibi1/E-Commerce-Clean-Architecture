namespace ECommerce.Application.Commands
{
    public sealed record CreateCustomerCommand(
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