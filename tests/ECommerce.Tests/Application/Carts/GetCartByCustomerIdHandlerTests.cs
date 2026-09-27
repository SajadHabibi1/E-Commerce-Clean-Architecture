using ECommerce.Application.DTOs;
using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Carts
{
    public class GetCartByCustomerIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnEmptyCart_WhenNoCartExists()
        {
            // Arrange
            var repository = new FakeCartRepository();
            var handler = new GetCartByCustomerIdHandler(repository);

            var query = new GetCartByCustomerIdQuery(Guid.NewGuid());

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);

            var returnedCart = Assert.IsType<CartDto>(result.Value);

            Assert.Empty(returnedCart.Items);
            Assert.Equal(0m, result.Value.TotalAmount);
            Assert.Equal(query.CustomerId, returnedCart.CustomerId);
            Assert.Equal(Guid.Empty, returnedCart.Id);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var repository = new FakeCartRepository();
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);
            await repository.AddAsync(cart);

            var cartItem = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                2,
                1000m
            );
            cart.AddItem(cartItem);

            var handler = new GetCartByCustomerIdHandler(repository);
            var query = new GetCartByCustomerIdQuery(customerId);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);
            var returnedCart = Assert.IsType<CartDto>(result.Value);
            var returnedItem = Assert.Single(returnedCart.Items);

            Assert.Equal(cart.Id, returnedCart.Id);
            Assert.Equal(customerId, returnedCart.CustomerId);
            Assert.Equal(2000m, returnedCart.TotalAmount);
            Assert.Equal(cartItem.Id, returnedItem.Id);
            Assert.Equal(2, returnedItem.Quantity);
            Assert.Equal(2000m, returnedItem.TotalPrice);
        }
    }
}