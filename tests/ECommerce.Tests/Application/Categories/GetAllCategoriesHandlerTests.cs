using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Categories
{
    public class GetAllCategoriesHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new GetAllCategoriesHandler(repository);

            // Act
            var result = await handler.HandleAsync(new GetAllCategoriesQuery());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnAllCategories_WhenCategoriesExist()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            
            var category = new Category(
                "Electeronic",
                "Electronics stuff"
            );
            await repository.AddAsync(category);

            var handler = new GetAllCategoriesHandler(repository);

            // Act
            var result = await handler.HandleAsync(new GetAllCategoriesQuery());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Single(result.Value!);

        }
    }
}