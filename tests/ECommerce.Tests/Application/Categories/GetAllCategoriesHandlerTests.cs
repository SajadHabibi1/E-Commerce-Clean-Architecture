using ECommerce.Application.DTOs;
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

            var categories = Assert.IsAssignableFrom<IReadOnlyList<CategoryDto>>(result.Value);
            Assert.Empty(categories);
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

            var categories = Assert.IsAssignableFrom<IReadOnlyList<CategoryDto>>(result.Value);

            var returnedCategory = Assert.Single(categories);

            Assert.Equal(category.Id, returnedCategory.Id);
            Assert.Equal(category.Name, returnedCategory.Name);
        }
    }
}