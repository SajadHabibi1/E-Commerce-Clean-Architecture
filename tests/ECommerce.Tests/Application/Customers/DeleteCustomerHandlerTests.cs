using ECommerce.Application.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Tests.Fakes;

namespace ECommerce.Tests.Application.Customers
{
    public class DeleteCustomerHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldSoftDeleteCustomer_WhenCommandIsValid()
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
            var handler = new DeleteCustomerHandler(repository);
            var cmd = new DeleteCustomerCommand(customer.Id);

            // Act
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(customer.IsDeleted);
            Assert.Equal(result.Value, customer.Id);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var repository = new FakeCustomerRepository();
            var handler = new DeleteCustomerHandler(repository);
            var cmd = new DeleteCustomerCommand(Guid.NewGuid());

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
            var handler = new DeleteCustomerHandler(repository);
            var cmd = new DeleteCustomerCommand(Guid.Empty);

            // Act 
            var result = await handler.HandleAsync(cmd);

            // Assert
            Assert.True(result.IsFailure);
        }
    }
}