namespace ECommerce.Application.Commands
{
    public sealed record CreateCategoryCommand(
        string Name,
        string? Description
    );
}