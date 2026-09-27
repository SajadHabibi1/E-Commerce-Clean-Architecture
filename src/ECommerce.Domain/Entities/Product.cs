using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities
{
    public class Product
    {
    public Guid Id { get; private set; }
    public string Name { get; private set;} = null!;
    public string? Description { get; private set; }
    public string ArticleNumber { get; private set; } = null!;
    public string ImageUrl { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public int StockQuantity { get; private set;}
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Product() { }

    public Product (Guid categoryId, string name, string? description, string articleNumber, string imageUrl, Money price, int stockQuantity)
        {
            var trimmedName = name.Trim();
            var trimmedArticleNumber = articleNumber.Trim();
            var trimmedImageUrl = imageUrl.Trim();
            EnsureValid(categoryId, trimmedName, trimmedArticleNumber, trimmedImageUrl, price, stockQuantity);

            Id = Guid.NewGuid();
            CategoryId = categoryId;
            Name = trimmedName;
            Description = description?.Trim();
            ArticleNumber = trimmedArticleNumber;
            ImageUrl = trimmedImageUrl;
            Price = price;
            StockQuantity = stockQuantity;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }

        private static void EnsureValid(Guid categoryId, string name, string articleNumber, string imageUrl, Money price, int stockQuantity)
        {
            if (categoryId == Guid.Empty)
            {
                throw new DomainException("CategoryId is required");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Product name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(articleNumber))
            {
                throw new DomainException("ArticleNumber cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new DomainException("ImageUrl is required");
            }

            if (price.Amount <= 0)
            {
                throw new DomainException("Price must be greater than 0");
            }

            if (stockQuantity < 0)
            {
                throw new DomainException("StockQuantity cannot be negative");
            }
        }

        public void Activate()
        {
            EnsureNotDeleted();

            if (IsActive)
            {
                throw new DomainException("Product is already active");
            }

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            EnsureNotDeleted();

            if (!IsActive)
            {
                throw new DomainException("Product is already inactive");
            }

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void EnsureNotDeleted()
        {
            if (IsDeleted)
            {
                throw new DomainException("The product is already deleted");
            }
        }

        public void Edit(Guid categoryId, string name, string? description, string articleNumber, string imageUrl, Money price, int stockQuantity)
        {
            EnsureNotDeleted();

            var trimmedName = name.Trim();
            var trimmedArticleNumber = articleNumber.Trim();
            var trimmedImageUrl = imageUrl.Trim();
            EnsureValid(categoryId, trimmedName, trimmedArticleNumber, trimmedImageUrl, price, stockQuantity);

            CategoryId = categoryId;
            Name = trimmedName;
            Description = description?.Trim();
            ArticleNumber = trimmedArticleNumber;
            ImageUrl = trimmedImageUrl;
            Price = price;
            StockQuantity = stockQuantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            EnsureNotDeleted();

            IsActive = false;
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            DeletedAt = DateTime.UtcNow;
        }

        public void ReduceStock(int quantity)
        {
            EnsureNotDeleted();

            if (quantity < 0)
            {
                throw new DomainException("Quantity cannot be negative");
            }

            if (quantity > StockQuantity)
            {
                throw new ProductOutOfStockException($"Not enough stock for product '{Name}'. Requested: {quantity}, available: {StockQuantity}");
            }

            StockQuantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}