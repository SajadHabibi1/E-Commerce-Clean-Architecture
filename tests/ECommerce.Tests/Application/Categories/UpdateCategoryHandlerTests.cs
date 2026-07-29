using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Categories
{
    public class UpdateCategoryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldUpdateCategory_WhenCommandIsValid()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var category = new Category(
                "Electronics",
                "Electronics stuff"
            );
            await repository.AddAsync(category);

            var handler = new UpdateCategoryHandler(repository);

            var cmd = new UpdateCategoryCommand(
                category.Id,
                "PC",
                "Gaming pc"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(result.Value, category.Id);
            Assert.Equal("PC", category.Name);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arange
            var repository = new FakeCategoryRepository();
            var handler = new UpdateCategoryHandler(repository);

            var cmd = new UpdateCategoryCommand(
                Guid.NewGuid(),
                "Electronics",
                "Electronics stuff"
            );

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
            var handler = new UpdateCategoryHandler(repository);
            var cmd = new UpdateCategoryCommand(
                Guid.Empty,
                "Electronics",
                "Electronics stuff"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenAnotherCategoryHasTheName()
        {
            // Arrange
            var respository = new FakeCategoryRepository();

            var category1 = new Category(
                "Electronic",
                ""
            );
            await respository.AddAsync(category1);

            var category2 = new Category(
                "Pc",
                ""
            );
            await respository.AddAsync(category2);

            var handler = new UpdateCategoryHandler(respository);

            var cmd = new UpdateCategoryCommand(
                category2.Id,
                "Electronic",
                ""
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldSucceed_WhenUpdatingWithoutChangingName()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var category = new Category(
                "Electronic",
                ""
            );
            await repository.AddAsync(category);

            var handler = new UpdateCategoryHandler(repository);

            var cmd = new UpdateCategoryCommand(
                category.Id,
                "Electronic",
                "Electronic stuff"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}