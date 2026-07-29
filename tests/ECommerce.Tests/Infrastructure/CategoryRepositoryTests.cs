using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure
{
    public class CategoryRepositoryTests
    {
        private ECommerceDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            return new ECommerceDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveCategory()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var category = new Category(
                "Electronics",
                "Electronics stuff"
            );

            // Act
            await repository.AddAsync(category);
            var savedCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == category.Id);

            // Assert
            Assert.NotNull(savedCategory);
            Assert.Equal(category.Id, savedCategory.Id);
            Assert.Equal(category.Name, savedCategory.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var category = new Category(
                "Electronics",
                "Electronics stuff"
            );
            await repository.AddAsync(category);

            // Act
            var result = await repository.GetByIdAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange 
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCategories_WhenCategoriesExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var category = new Category(
                "Laptop",
                "All laptop"
            );
            await repository.AddAsync(category);
            
            var category2 = new Category(
                "Pc",
                "Gaming pc"
            );
            await repository.AddAsync(category2);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(repository);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Laptop");
            Assert.Contains(result, c => c.Name == "Pc");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var category = new Category(
                "Laptop",
                "All laptop"
            );
            await repository.AddAsync(category);

            // Act
            category.Edit(
                "Pc",
                "Gaming pc"
            );
            await repository.UpdateAsync(category);
            
            var updatedCategory = await repository.GetByIdAsync(category.Id);

            // Assert
            Assert.NotNull(updatedCategory);
            Assert.Equal("Pc", updatedCategory.Name);
            Assert.NotNull(updatedCategory.UpdatedAt);
        }

        [Fact]
        public async Task GetAllAsync_ShouldNotReturnSoftDeletedCategories()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var category = new Category(
                "Pc",
                "Gaming pc"
            );
            await repository.AddAsync(category);

            // Act
            category.SoftDelete();
            await repository.UpdateAsync(category);
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}