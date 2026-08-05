namespace ECommerce.Application.DTOs
{
    public sealed record CustomerDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? Street,
        string? City,
        string? PostalCode,
        string? Country,
        bool IsActive
    );
}