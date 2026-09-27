using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_ShouldCreateProduct_WhenDataIsValid()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "laptop.jpg";
            var price = 1000m;
            var stockQuantity = 10;

            // Act
            var product = new Product(categoryId, 
            name, 
            description, 
            articleNumber,
            imageUrl,
            new Money(price, "SEK"),
            stockQuantity);

            // Assert
            Assert.Equal(categoryId, product.CategoryId);
            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Equal(articleNumber, product.ArticleNumber);
            Assert.Equal(imageUrl, product.ImageUrl);
            Assert.Equal(price, product.Price.Amount);
            Assert.Equal(stockQuantity, product.StockQuantity);

            Assert.True(product.IsActive);
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.NotEqual(default, product.CreatedAt);
            Assert.Null(product.UpdatedAt);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCategoryIdIsEmpty()
        {
            // Arrange
            var categoryId = Guid.Empty;
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "laptop.jpg";
            var price = 1000m;
            var stockQuantity = 10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(categoryId, 
            name, 
            description, 
            articleNumber,
            imageUrl,
            new Money(price, "SEK"),
            stockQuantity));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var name = "";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "laptop.jpg";
            var price = 1000m;
            var stockQuantity = 10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(categoryId, 
            name, 
            description, 
            articleNumber,
            imageUrl,
            new Money(price, "SEK"),
            stockQuantity));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenArticleNumberIsEmpty()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "";
            var imageUrl = "laptop.jpg";
            var price = 1000m;
            var stockQuantity = 10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(categoryId, 
            name, 
            description, 
            articleNumber,
            imageUrl,
            new Money(price, "SEK"),
            stockQuantity));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenImageUrlIsEmpty()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "";
            var price = 1000m;
            var stockQuantity = 10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(categoryId, 
            name, 
            description, 
            articleNumber,
            imageUrl,
            new Money(price, "SEK"),
            stockQuantity));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenPriceIsZeroOrNegative()
        {
            // Arrang
            var categoryId = Guid.NewGuid();
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "laptop.jpg";
            var price = 0;
            var stockQuantity = 10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(
                categoryId,
                name,
                description,
                articleNumber,
                imageUrl,
                new Money(price, "SEK"),
                stockQuantity
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenStockQuantityIsNegative()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var name = "Laptop";
            var description = "Gaming laptop";
            var articleNumber = "ART-123";
            var imageUrl = "laptop.jpg";
            var price = 1000m;
            var stockQuantity = -10;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Product(
                categoryId,
                name,
                description,
                articleNumber,
                imageUrl,
                new Money(price, "SEK"),
                stockQuantity
            ));
        }

        [Fact]
        public void Edit_ShouldUpdateProduct_WhenDataIsValid ()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(categoryId, "Laptop", "Gaming laptop", "ART-123", "laptop.jpg", new Money(1000m, "SEK"), 10);

            // Act
            product.Edit(
                categoryId,
                "Camputer",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(5000m, "SEK"),
                15
            );

            // Assert
            Assert.Equal(categoryId, product.CategoryId);
            Assert.Equal("Camputer", product.Name);
            Assert.Equal("Office camputer", product.Description);
            Assert.Equal("ART-999", product.ArticleNumber);
            Assert.Equal("camputer.jpg", product.ImageUrl);
            Assert.Equal(5000m, product.Price.Amount);
            Assert.Equal(15, product.StockQuantity);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Edit_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(categoryId, 
            "Laptop", 
            "Gaming laptop", 
            "ART-123", 
            "laptop.jpg", 
            new Money(1000m, "SEK"), 
            10);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            product.Edit(
                categoryId,
                "",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(5000m, "SEK"),
                15
            ));
        }

        [Fact]
        public void Edit_ShouldThrowException_WhenPriceIsZero()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            product.Edit(
                categoryId,
                "Camputer",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(0m, "SEK"),
                15
            ));
        }

        [Fact]
        public void Edit_ShouldThrowException_WhenPriceIsNegative()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            product.Edit(
                categoryId,
                "Camputer",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(-100m, "SEK"),
                15
            ));
        }

        [Fact]
        public void Edit_ShouldThrowException_WhenCategoryIdIsEmpty()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            product.Edit(
                Guid.Empty,
                "Camputer",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(1000m, "SEK"),
                15
            ));
        }

        [Fact]
        public void Edit_ShouldThrowException_WhenStockQuantityIsNegative()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            product.Edit(
                categoryId,
                "Camputer",
                "Office camputer",
                "ART-999",
                "camputer.jpg",
                new Money(1000m, "SEK"),
                -10
            ));
        }

        [Fact]
        public void Activate_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act
            product.Deactivate();
            product.Activate();

            // Assert
            Assert.True(product.IsActive);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act
            product.Deactivate();

            // Assert
            Assert.False(product.IsActive);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void SoftDelete_ShouldMarkProductAsDeleted()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product(
                categoryId, 
                "Laptop", 
                "Gaming laptop", 
                "ART-123", 
                "laptop.jpg", 
                new Money(1000m, "SEK"), 
                10);

            // Act
            product.SoftDelete();

            // Assert
            Assert.True(product.IsDeleted);
            Assert.False(product.IsActive);
            Assert.NotNull(product.DeletedAt);
        }

        [Fact]
        public void ReduceStock_ShouldDecreaseStockQuantity_WhenEnoughStockIsAvailable()
        {
            // Arrange
            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "latop.jpg",
                new Money(1000m, "SEK"),
                10
            );

            // Act
            product.ReduceStock(3);

            // Assert
            Assert.Equal(7, product.StockQuantity);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void ReduceStock_ShouldThrowProductOutOfStockException_WhenNotEnoughStock()
        {
            // Arrange
            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "latop.jpg",
                new Money(1000m, "SEK"),
                1
            );

            // Act & Assert
            Assert.Throws<ProductOutOfStockException>(() =>
            product.ReduceStock(3));
        }

        [Fact]
        public void ReduceStock_ShouldRejectNegativeQuantity_WithoutChangingStock()
        {
            // Arrange
            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                10
            );

            // Act & Assert
            Assert.Throws<DomainException>(() => product.ReduceStock(-1));
            Assert.Equal(10, product.StockQuantity);
        }
    }
}