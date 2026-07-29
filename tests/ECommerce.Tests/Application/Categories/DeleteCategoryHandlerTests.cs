using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Categories
{
    public class DeleteCategoryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldSoftDeleteCategory_WhenCommandIsValid()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var category = new Category(
                "Electronics",
                "Electronics stuff"
            );
            await repository.AddAsync(category);

            var handler = new DeleteCategoryHandler(repository);
            var cmd = new DeleteCategoryCommand(category.Id);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(category.IsDeleted);
            Assert.Equal(result.Value, category.Id);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new DeleteCategoryHandler(repository);
            var cmd = new DeleteCategoryCommand(Guid.NewGuid());

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenIdIsEmpty()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new DeleteCategoryHandler(repository);
            var cmd = new DeleteCategoryCommand(Guid.Empty);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}