namespace ECommerce.Application.DTOs
{
    public sealed record CategoryDto(
        Guid Id,
        string Name,
        string? Description,
        bool IsActive
    );
}