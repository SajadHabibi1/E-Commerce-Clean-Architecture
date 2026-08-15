namespace ECommerce.Application.DTOs
{
    public sealed record CartDto(
        Guid Id,
        Guid CustomerId,
        decimal TotalAmount,
        IReadOnlyList<CartItemDto> Items
    );
}