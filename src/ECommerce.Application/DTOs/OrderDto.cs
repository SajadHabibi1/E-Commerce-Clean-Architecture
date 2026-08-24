using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs
{
    public sealed record OrderDto(
        Guid Id,
        Guid CustomerId,
        string OrderNumber,
        OrderStatus Status,
        PaymentStatus PaymentStatus,
        decimal TotalAmount,
        string ShippingStreet,
        string ShippingCity,
        string ShippingPostalCode,
        string ShippingCountry,
        string BillingStreet,
        string BillingCity,
        string BillingPostalCode,
        string BillingCountry,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        IReadOnlyList<OrderItemDto> Items
    );
}