using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Categories
{
    public class CreateCategoryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenCommandIsValid()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new CreateCategoryHandler(repository);

            var cmd = new CreateCategoryCommand(
                "Electronics",
                "Elecronics stuff"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            Assert.Single(repository.Categories);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenNameIsEmpty()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new CreateCategoryHandler(repository);

            var cmd = new CreateCategoryCommand(
                "",
                "Electronic stuff"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Empty(repository.Categories);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenAnotherCategoryHasTheName()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            
            var category = new Category(
                "Electronic",
                "Electronic stuff"
            );
            await repository.AddAsync(category);

            var handler = new CreateCategoryHandler(repository);

            var cmd = new CreateCategoryCommand(
                "Electronic",
                "Electronic stuff"
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}