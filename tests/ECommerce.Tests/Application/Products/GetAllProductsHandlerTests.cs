using ECommerce.Application.DTOs;
using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Products
{
    public class GetAllProductsHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WithEmptyList_WithNoProductsExists()
        {
            // Arrange
            var repository = new FakeProductRepository();

            var handler = new GetAllProductsHandler(repository);
            var query = new GetAllProductsQuery();

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);

            var products = Assert.IsAssignableFrom<IReadOnlyList<ProductDto>>(result.Value);
            Assert.Empty(products);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WithProducts_WhenProductsExist()
        {
            // Arrange
            var repository = new FakeProductRepository();
            
            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                10
            );
            repository.Products.Add(product);

            var product2 = new Product(
                Guid.NewGuid(),
                "Mouse",
                "Gaming mouse",
                "ART-002",
                "mouse.jpg",
                new Money(100m, "SEK"),
                15
            );
            repository.Products.Add(product2);

            var handler = new GetAllProductsHandler(repository);
            var query = new GetAllProductsQuery();

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);

            var products = Assert.IsAssignableFrom<IReadOnlyList<ProductDto>>(result.Value);
            Assert.NotEmpty(products);
            Assert.Equal(2, products.Count);
            Assert.Equal("Laptop", products[0].Name);
            Assert.Equal("Mouse", products[1].Name);
        }
    }
}