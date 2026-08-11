using ECommerce.Application.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Customers
{
    public class GetAllCustomersHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new GetAllCustomersHandler(repository);

            // Act
            var result = await handler.HandleAsync(new GetAllCustomersQuery());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value!);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnAllCustomers_WhenCustomersExist()
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

            var handler = new GetAllCustomersHandler(repository);

            // Act
            var result = await handler.HandleAsync(new GetAllCustomersQuery());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value!);
            Assert.Single(result.Value!);
        }
    }
}