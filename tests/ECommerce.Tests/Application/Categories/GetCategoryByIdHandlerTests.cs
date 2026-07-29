using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Categories
{
    public class GetCategoryByIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenCategoryExists()
        {
            // Arrange
            var repository = new FakeCategoryRepository();

            var category = new Category(
                "Electronic",
                "Electronics stuff"
            );
            await repository.AddAsync(category);

            var handler = new GetCategoryByIdHandler(repository);
            var query = new GetCategoryByIdQuery(category.Id);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(result.Value.Id, category.Id);
            Assert.Equal(result.Value.Name, category.Name);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new GetCategoryByIdHandler(repository);
            var query = new GetCategoryByIdQuery(Guid.NewGuid());

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenIdIsEmpty()
        {
            // Arrange
            var repository = new FakeCategoryRepository();
            var handler = new GetCategoryByIdHandler(repository);
            var query = new GetCategoryByIdQuery(Guid.Empty);

            // Act 
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}