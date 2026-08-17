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
            Assert.Empty(result.Value.Items);
            Assert.Equal(0, result.Value.TotalAmount);
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
            Assert.NotEmpty(result.Value.Items);
            Assert.Equal(2000m, result.Value.TotalAmount);
        }
    }
}