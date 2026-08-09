using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Customers
{
    public class GetCustomerByIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnSuccess_WhenCustomerExists()
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

            var handler = new GetCustomerByIdHandler(repository);
            var query = new GetCustomerByIdQuery(customer.Id);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(result.Value.Id, customer.Id);
            Assert.Equal(result.Value.Email, customer.Email);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new GetCustomerByIdHandler(repository);
            var query = new GetCustomerByIdQuery(Guid.NewGuid());

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenIdIsEmpty()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new GetCustomerByIdHandler(repository);
            var query = new GetCustomerByIdQuery(Guid.Empty);

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}