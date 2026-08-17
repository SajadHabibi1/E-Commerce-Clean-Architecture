using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Queries
{
    public sealed class GetCartByCustomerIdHandler
    {
        private readonly ICartRepository _cartRepository;

        public GetCartByCustomerIdHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Result<CartDto>> HandleAsync(GetCartByCustomerIdQuery query, CancellationToken ct = default)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(query.CustomerId, ct);

            if (cart is null)
            {
                return Result<CartDto>.Success(new CartDto(
                    Guid.Empty,
                    query.CustomerId,
                    0,
                    new List<CartItemDto>()
                ));
            }

            var dto = cart.ToDto();
            
            return Result<CartDto>.Success(dto);
        }
    }
}