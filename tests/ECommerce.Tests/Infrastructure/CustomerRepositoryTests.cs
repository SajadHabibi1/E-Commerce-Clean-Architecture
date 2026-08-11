using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure
{
    public class CustomerRepositoryTests
    {
        private ECommerceDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            return new ECommerceDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveCustomer()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
                null
            );
            await repository.AddAsync(customer);

            // Act
            var savedCustomer = await context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            // Assert
            Assert.NotNull(savedCustomer);
            Assert.Equal(customer.Id, savedCustomer.Id);
            Assert.Equal(customer.Email, savedCustomer.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
                null
            );
            await repository.AddAsync(customer);

            // Act
            var result = await repository.GetByIdAsync(customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCustomers_WhenCustomersExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);
            var customer1 = new Customer(
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null
            );
            await repository.AddAsync(customer1);

            var customer2 = new Customer(
                "Jane",
                "Doe",
                "janedoe@example.com",
                "0731234567",
                null
            );
            await repository.AddAsync(customer2);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Email == "johndoe@example.com");
            Assert.Contains(result, c => c.Email == "janedoe@example.com");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer_WhenCustomerExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer(
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null
            );
            await repository.AddAsync(customer);

            // Act
            customer.Edit(
                "Jane",
                "Doe",
                customer.Email,
                customer.PhoneNumber,
                null
            );
            await repository.UpdateAsync(customer);
            var updatedCustomer = await repository.GetByIdAsync(customer.Id);

            // Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("Jane", updatedCustomer.FirstName);
            Assert.NotNull(customer.UpdatedAt);
        }

        [Fact]
        public async Task GetAllAsync_ShouldNotReturnSoftDeletedCustomers()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new CustomerRepository(context);
            var customer = new Customer(
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null
            );
            await repository.AddAsync(customer);

            // Act
            customer.SoftDelete();
            await repository.UpdateAsync(customer);
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

    }
}