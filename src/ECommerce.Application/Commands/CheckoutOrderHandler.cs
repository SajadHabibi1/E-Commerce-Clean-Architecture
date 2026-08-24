using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Application.Commands
{
    public sealed class CheckoutOrderHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Result<Guid>> HandleAsync( CheckoutOrderCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            var cart = await _cartRepository.GetByCustomerIdAsync(cmd.CustomerId, ct);

            if (cart is null)
            {
                return Result<Guid>.NotFound("Cart not found");
            }

            if (cart.CartItems.Count == 0)
            {
                return Result<Guid>.Failure("Cart is empty");
            }

            try
            {
                var shippingAddress = new Address(cmd.ShippingStreet, cmd.ShippingCity, cmd.ShippingPostalCode, cmd.ShippingCountry);

                var billingAddress = new Address(cmd.BillingStreet, cmd.BillingCity, cmd.BillingPostalCode, cmd.BillingCountry);

                var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
                
                var order = new Order(cmd.CustomerId, orderNumber, shippingAddress, billingAddress);

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _productRepository.GetByIdAsync(cartItem.ProductId, ct);

                    if (product is null)
                    {
                        return Result<Guid>.NotFound($"Product {cartItem.ProductId} not found");
                    }

                    var orderItem = new OrderItem(
                        order.Id,
                        product.Id,
                        product.Name,
                        product.ArticleNumber,
                        cartItem.Quantity,
                        product.Price.Amount
                    );

                    order.AddOrderItem(orderItem, product);
                }
                await _orderRepository.AddAsync(order, ct);

                cart.Clear();
                await _cartRepository.UpdateAsync(cart, ct);

                return Result<Guid>.Success(order.Id);
            }

            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}