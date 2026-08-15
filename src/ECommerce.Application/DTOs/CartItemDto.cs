namespace ECommerce.Application.DTOs
{
    public sealed record CartItemDto(
        Guid Id,
        Guid CartId,
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
    );
}