using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class AddItemToCartHandler
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public AddItemToCartHandler(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Result<Guid>> HandleAsync(AddItemToCartCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            try
            {
                var product = await _productRepository.GetByIdAsync(cmd.ProductId, ct);

                if (product is null)
                {
                    return Result<Guid>.NotFound("Product not found");
                }

                var cart = await _cartRepository.GetByCustomerIdAsync(cmd.CustomerId, ct);

                var isNewCart = cart is null;

                if (cart is null)
                {
                    cart = new Cart(cmd.CustomerId);
                }

                var cartItem = new CartItem(
                    cart.Id,
                    cmd.ProductId,
                    cmd.Quantity,
                    product.Price.Amount
                );
                cart.AddItem(cartItem);

                if (isNewCart)
                {
                    await _cartRepository.AddAsync(cart, ct);
                }
                else
                {
                    await _cartRepository.UpdateAsync(cart, ct);
                }

                return Result<Guid>.Success(cart.Id);
            }
            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}