using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Carts
{
    public class UpdateCartItemQuantityHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldUpdateQuantity_WhenCartAndItemExist()
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
            var handler = new UpdateCartItemQuantityHandler(repository);
            var cmd = new UpdateCartItemQuantityCommand(customerId, cartItem.Id, 5);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCartDoesNotExist()
        {
            // Arrange
            var repository = new FakeCartRepository();
            var handler = new UpdateCartItemQuantityHandler(repository);
            var cmd = new UpdateCartItemQuantityCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                5
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenItemDoesNotExistInCart()
        {
            // Arrange
            var repository = new FakeCartRepository();
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);
            await repository.AddAsync(cart);

            var handler = new UpdateCartItemQuantityHandler(repository);
            var cmd = new UpdateCartItemQuantityCommand(customerId, Guid.NewGuid(), 5);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenQuantityIsZeroOrNegative()
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
            var handler = new UpdateCartItemQuantityHandler(repository);
            var cmd = new UpdateCartItemQuantityCommand(customerId, cartItem.Id, 0);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}