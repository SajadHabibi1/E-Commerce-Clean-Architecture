using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class RemoveItemFromCartHandler
    {
        private readonly ICartRepository _cartRepository;

        public RemoveItemFromCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Result<Guid>> HandleAsync(RemoveItemFromCartCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            try
            {
                var cart = await _cartRepository.GetByCustomerIdAsync(cmd.CustomerId, ct);
                
                if (cart is null)
                {
                    return Result<Guid>.NotFound("Cart not found");
                }

                cart.RemoveItem(cmd.CartItemId);

                await _cartRepository.UpdateAsync(cart, ct);

                return Result<Guid>.Success(cart.Id);
            }
            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}