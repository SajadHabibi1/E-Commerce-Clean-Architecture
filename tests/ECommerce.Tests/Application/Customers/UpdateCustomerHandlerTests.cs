using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Customers
{
    public class UpdateCustomerHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldUpdateCustomer_WhenCommandIsValid()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var customer = new Customer(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
                null
            );
            await repository.AddAsync(customer);

            var handler = new UpdateCustomerHandler(repository);
            var cmd = new UpdateCustomerCommand(
                customer.Id,
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null,
                null,
                null,
                null
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(result.Value, customer.Id);
            Assert.Equal("John", customer.FirstName);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new UpdateCustomerHandler(repository);
            var cmd = new UpdateCustomerCommand(
                Guid.NewGuid(),
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null,
                null,
                null,
                null
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
            var repository = new FakeCustomerRepository();
            var handler = new UpdateCustomerHandler(repository);
            var cmd = new UpdateCustomerCommand(
                Guid.Empty,
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null,
                null,
                null,
                null
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenAnotherCustomerHasTheEmail()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
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

            var handler = new UpdateCustomerHandler(repository);

            var cmd = new UpdateCustomerCommand(
                customer1.Id,
                "John",
                "Doe",
                "janedoe@example.com",
                "0731234567",
                null,
                null,
                null,
                null
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldSucceed_WhenUpdatingWithoutChangingEmail()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var customer = new Customer(
                "John",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null
            );
            await repository.AddAsync(customer);
            
            var handler = new UpdateCustomerHandler(repository);
            var cmd = new UpdateCustomerCommand(
                customer.Id,
                "Jane",
                "Doe",
                "johndoe@example.com",
                "0731234567",
                null,
                null,
                null,
                null
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}