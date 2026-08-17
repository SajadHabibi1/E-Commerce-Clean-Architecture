using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public static class CartMappingExtensions
    {
        public static CartItemDto ToDto(this CartItem item)
        {
            return new CartItemDto(
                item.Id,
                item.CartId,
                item.ProductId,
                item.Quantity,
                item.UnitPrice,
                item.Quantity * item.UnitPrice
            );
        }

        public static CartDto ToDto(this Cart cart)
        {
            return new CartDto(
                cart.Id,
                cart.CustomerId,
                cart.TotalAmount,
                cart.CartItems.Select(item => item.ToDto()).ToList()
            );
        }
    }
}