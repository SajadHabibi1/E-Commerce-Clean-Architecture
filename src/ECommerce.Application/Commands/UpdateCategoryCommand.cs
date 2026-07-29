namespace ECommerce.Application.Commands
{
    public sealed record UpdateCategoryCommand(
        Guid Id,
        string Name,
        string? Description
    );
}