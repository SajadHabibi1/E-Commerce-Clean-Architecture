using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Carts
{
    public class AddItemToCartHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCreateNewCartAndAddItem_WhenNoCartExists()
        {
            // Arrange
            var cartRepository = new FakeCartRepository();
            var productRepository = new FakeProductRepository();

            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                10
            );
            productRepository.Products.Add(product);

            var handler = new AddItemToCartHandler(cartRepository, productRepository);

            var cmd = new AddItemToCartCommand(
                Guid.NewGuid(),
                product.Id,
                2
            );

            // Act
            var result = await handler.HandleAsync(cmd);
            var cart = cartRepository.Carts.First();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(cartRepository.Carts);
            Assert.Equal(2, cart.CartItems.First().Quantity);
        }

        [Fact]
        public async Task HandleAsync_ShouldAddItemToExistingCart_WhenCartAlreadyExists()
        {
            // Arrange
            var cartRepository = new FakeCartRepository();
            var productRepository = new FakeProductRepository();

            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                10
            );
            productRepository.Products.Add(product);

            var customerId = Guid.NewGuid();

            var cart = new Cart(
                customerId
            );
            await cartRepository.AddAsync(cart);

            var handler = new AddItemToCartHandler(cartRepository, productRepository);

            var cmd = new AddItemToCartCommand(
                customerId,
                product.Id,
                2
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(cartRepository.Carts);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            // Arrange
            var cartRepository = new FakeCartRepository();
            var productRepository = new FakeProductRepository();
            var handler = new AddItemToCartHandler(cartRepository, productRepository);

            var cmd = new AddItemToCartCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                3
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}