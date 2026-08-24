using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public static class OrderMappingExtensions
    {
        public static OrderItemDto ToDto(this OrderItem item)
        {
            return new OrderItemDto(
                item.Id,
                item.OrderId,
                item.ProductId,
                item.ProductName,
                item.ArticleNumber,
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice
            );
        }

        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto(
                order.Id,
                order.CustomerId,
                order.OrderNumber,
                order.Status,
                order.PaymentStatus,
                order.TotalAmount,
                order.ShippingAddress.Street,
                order.ShippingAddress.City,
                order.ShippingAddress.PostalCode,
                order.ShippingAddress.Country,
                order.BillingAddress.Street,
                order.BillingAddress.City,
                order.BillingAddress.PostalCode,
                order.BillingAddress.Country,
                order.CreatedAt,
                order.UpdatedAt,
                order.OrderItems.Select(item => item.ToDto()).ToList()
            );
        }
    }
}