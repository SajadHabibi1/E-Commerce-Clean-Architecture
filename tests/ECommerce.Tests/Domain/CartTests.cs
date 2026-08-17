using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Tests.Domain
{
    public class CartTests
    {
        [Fact]
        public void Constructor_ShouldCreateCart_WhenCustomerIdIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            // Act 
            var cart = new Cart(
                customerId
            );

            // Assert
            Assert.Equal(customerId, cart.CustomerId);
            Assert.NotEqual(default, cart.CreatedAt);
            Assert.Null(cart.UpdatedAt);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCustomerIdIsEmpty()
        {
            // Arrange
            var customerId = Guid.Empty;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Cart(
                customerId
            ));
        }

        [Fact]
        public void AddItem_ShouldAddItemToCart_WhenItemIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(
                customerId
            );

            var item = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                10,
                1000
            );

            // Act
            cart.AddItem(item);

            // Assert
            Assert.Single(cart.CartItems);
            Assert.Contains(item, cart.CartItems);
            Assert.Equal(10000, cart.TotalAmount);
            Assert.NotNull(cart.UpdatedAt);
        }

        [Fact]
        public void AddItem_ShouldThrowException_WhenItemIsNull()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(
                customerId
            );

            CartItem? item = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            cart.AddItem(item!));
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemFromCart_WhenItemExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(
                customerId
            );

            var item = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                10,
                1000
            );

            cart.AddItem(item);

            // Act
            cart.RemoveItem(item.Id);

            // Assert
            Assert.Empty(cart.CartItems);
            Assert.Equal(0, cart.TotalAmount);
            Assert.NotNull(cart.UpdatedAt);
        }

        [Fact]
        public void RemoveItem_ShouldThrowException_WhenItemDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(
                customerId
            );

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            cart.RemoveItem(Guid.NewGuid()));
        }

        [Fact]
        public void ChangeItemQuantity_ShouldUpdateQuantity_WhenItemExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);

            var item = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                10,
                1000
            );
            cart.AddItem(item);

            // Act
            cart.ChangeItemQuantity(item.Id, 5);

            // Assert
            Assert.Equal(5, item.Quantity);
            Assert.NotNull(cart.UpdatedAt);
        }

        [Fact]
        public void ChangeItemQuantity_ShouldRecalculateTotalAmount_WhenQuantityChanges()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);

            var item = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                10,
                1000
            );
            cart.AddItem(item);

            // Act
            cart.ChangeItemQuantity(item.Id, 3);

            // Assert
            Assert.Equal(3000, cart.TotalAmount);
        }

        [Fact]
        public void ChangeItemQuantity_ShouldThrowException_WhenItemDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            cart.ChangeItemQuantity(Guid.NewGuid(), 5));
        }

        [Fact]
        public void ChangeItemQuantity_ShouldThrowException_WhenQuantityIsZeroOrNegative()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);

            var item = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                10,
                1000
            );
            cart.AddItem(item);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            cart.ChangeItemQuantity(item.Id, 0));
        }
    }
}