namespace ECommerce.Application.DTOs
{
    public sealed record OrderItemDto(
        Guid Id,
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        string ArticleNumber,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
    );
}