using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure
{
    public class CartRepositoryTests
    {
        private ECommerceDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            return new ECommerceDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveCart()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CartRepository(context);
            var cart = new Cart(Guid.NewGuid());

            // Act
            await repository.AddAsync(cart);
            var savedCart = await context.Carts.FirstOrDefaultAsync(c => c.Id == cart.Id);

            // Assert
            Assert.NotNull(savedCart);
            Assert.Equal(cart.CustomerId, savedCart.CustomerId);
        }

        [Fact]
        public async Task GetByCustomerIdAsync_ShouldReturnCartWithItems_WhenCartExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CartRepository(context);
            
            var customerId = Guid.NewGuid();
            var cart = new Cart(customerId);
            var cartItem = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                2,
                1000m);
            cart.AddItem(cartItem);
            await repository.AddAsync(cart);

            // Act
            var result = await repository.GetByCustomerIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.CartItems);
        }

        [Fact]
        public async Task GetByCustomerIdAsync_ShouldReturnNull_WhenCartDoesNotExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CartRepository(context);

            // Act
            var result = await repository.GetByCustomerIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges_WhenCartIsUpdated()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CartRepository(context);
            
            var cart = new Cart(Guid.NewGuid());
            await repository.AddAsync(cart);


            // Act
            var cartItem = new CartItem(
                cart.Id,
                Guid.NewGuid(),
                3,
                500m
            );
            cart.AddItem(cartItem);
            await repository.UpdateAsync(cart);
            var result = await repository.GetByCustomerIdAsync(cart.CustomerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.CartItems);
            Assert.Equal(1500m, result.TotalAmount);
        }
    }
}