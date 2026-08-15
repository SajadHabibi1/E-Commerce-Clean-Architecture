using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Carts
{
    public class RemoveItemFromCartHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldRemoveItem_WhenCartAndItemExist()
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
            var handler = new RemoveItemFromCartHandler(repository);
            var cmd = new RemoveItemFromCartCommand(customerId, cartItem.Id);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(cart.CartItems);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCartDoesNotExist()
        {
            // Arrange
            var repository = new FakeCartRepository();
            var handler = new RemoveItemFromCartHandler(repository);
            var cmd = new RemoveItemFromCartCommand(
                Guid.NewGuid(),
                Guid.NewGuid()
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

            var handler = new RemoveItemFromCartHandler(repository);
            var cmd = new RemoveItemFromCartCommand(customerId, Guid.NewGuid());

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}