using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Customers
{
    public class CreateCustomerHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenCommandIsValid()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new CreateCustomerHandler(repository);

            var cmd = new CreateCustomerCommand(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
                null,
                null,
                null,
                null
            );

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            Assert.Single(repository.Customers);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenEmailIsInvalid()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new CreateCustomerHandler(repository);

            var cmd = new CreateCustomerCommand(
                "Sajad",
                "Habibi",
                "sajadhabibiexample.com",
                "0721234567",
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
        public async Task HandleAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
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

            var handler = new CreateCustomerHandler(repository);

            var cmd = new CreateCustomerCommand(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
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
        public async Task HandleAsync_ShouldCreateCustomerWithAddress_WhenAddressFieldsAreProvided()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new CreateCustomerHandler(repository);
            var cmd = new CreateCustomerCommand(
                "Sajad",
                "Habibi",
                "sajadhabibi@example.com",
                "0721234567",
                "Storgatan",
                "Stockholm",
                "11122",
                "Sweden"
            );

            // Act
            var result = await handler.HandleAsync(cmd);
            var savedCustomer = repository.Customers.First();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(savedCustomer.Address);
            Assert.Equal("Stockholm", savedCustomer.Address.City);


        }
    }
}