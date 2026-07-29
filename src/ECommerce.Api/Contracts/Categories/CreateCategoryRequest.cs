namespace ECommerce.Api.Contracts.Categories
{
    public sealed record CreateCategoryRequest(
        string Name,
        string? Description
    );
}