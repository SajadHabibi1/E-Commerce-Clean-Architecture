using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void Constructor_ShouldCreateOrder_WhenDataIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderNumber = "ORD-001";
            var shippingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");
            var billingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");

            // Act
            var order = new Order(
                customerId,
                orderNumber,
                shippingAddress,
                billingAddress
            );

            // Assert
            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(orderNumber, order.OrderNumber);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.Equal(0m, order.TotalAmount);
            Assert.Equal(shippingAddress, order.ShippingAddress);
            Assert.Equal(billingAddress, order.BillingAddress);
            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.NotEqual(default, order.CreatedAt);
            Assert.Null(order.UpdatedAt);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCustomerIdIsEmpty()
        {
            // Arrange
            var customerId = Guid.Empty;
            var orderNumber = "ORD-001";
            var shippingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");
            var billingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Order(
                customerId,
                orderNumber,
                shippingAddress,
                billingAddress
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenOrderNumberIsEmpty()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderNumber = "";
            var shippingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");
            var billingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Order(
                customerId,
                orderNumber,
                shippingAddress,
                billingAddress
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenShippingAddressIsNull()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderNumber = "ORD-001";
            Address shippingAddress = null!;
            var billingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Order(
                customerId,
                orderNumber,
                shippingAddress,
                billingAddress
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenBillingAddressIsNull()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderNumber = "ORD-001";
            var shippingAddress = new Address("Storgatan 1", "Stockholm", "11122", "Sweden");
            Address billingAddress = null!;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            new Order(
                customerId,
                orderNumber,
                shippingAddress,
                billingAddress
            ));
        }

        [Fact]
        public void AddOrderItem_ShouldAddItemToOrder()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );
            var orderItem = new OrderItem(
                order.Id,
                Guid.NewGuid(),
                "Laptop",
                "ART-123",
                2,
                1000m
            );

            var product = new Product(
                Guid.NewGuid(),
                "Laptap",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m ,"SEK"),
                100
            );

            // Act
            order.AddOrderItem(orderItem, product);

            // Assert
            Assert.Single(order.OrderItems);
            Assert.Contains(orderItem,order.OrderItems);
            Assert.Equal(2000m, order.TotalAmount);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void AddOrderItem_ShouldThrowException_WhenItemIsNull()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            var product = new Product(
                Guid.NewGuid(),
                "Laptap",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                100
            );

            OrderItem? orderItem = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            order.AddOrderItem(orderItem!,product)
            );
        }

        [Fact]
        public void AddOrderItem_ShouldUpdateTotalAmount_WhenMultipleItemsAreAdded()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            var orderItem = new OrderItem(
                order.Id,
                Guid.NewGuid(),
                "Laptop", 
                "ART-123",
                2,
                1000m
            );
            var orderItem2 = new OrderItem(
                order.Id,
                Guid.NewGuid(),
                "Mouse",
                "ART-321",
                1,
                500m
            );

            var product = new Product(
                Guid.NewGuid(),
                "Laptap",
                "Gaming laptop",
                "ART-001",
                "laptop.jpg",
                new Money(1000m, "SEK"),
                100
            );

            var product2 = new Product(
                Guid.NewGuid(),
                "Mouse",
                "Gaming mouse",
                "ART-002",
                "mouse.jpg",
                new Money(100m, "SEK"),
                100
            );
            // Act
            order.AddOrderItem(orderItem, product);
            order.AddOrderItem(orderItem2, product2);

            // Assert
            Assert.Equal(2, order.OrderItems.Count);
            Assert.Contains(orderItem,order.OrderItems);
            Assert.Contains(orderItem2, order.OrderItems);
            Assert.Equal(2500m, order.TotalAmount);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsPaid_ShouldChangeStatusToPaid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();

            // Assert
            Assert.Equal(PaymentStatus.Paid, order.PaymentStatus);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsPaid_ShouldThrowException_WhenOrderIsNotPending()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act 
            order.MarkAsPaid();

            //Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.MarkAsPaid());
        }

        [Fact]
        public void MarkAsProcessing_ShouldChangeStatusToProcessing()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();
            order.MarkAsProcessing();

            // Assert
            Assert.Equal(OrderStatus.Processing, order.Status);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsProcessing_ShouldThrowException_WhenOrderIsNotPaid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act & Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.MarkAsProcessing());
        }

        [Fact]
        public void MarkAsShipped_ShouldChangeStatusToShipped()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();
            order.MarkAsProcessing();
            order.MarkAsShipped();

            // Assert
            Assert.Equal(OrderStatus.Shipped, order.Status);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsShipped_ShouldThrowException_WhenOrderIsNotProcessing()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act & Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.MarkAsShipped());
        }

        [Fact]
        public void MarkAsDelivered_ShouldChangeStatusToDelivered()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();
            order.MarkAsProcessing();
            order.MarkAsShipped();
            order.MarkAsDelivered();

            // Assert
            Assert.Equal(OrderStatus.Delivered, order.Status);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsDelivered_ShouldThrowException_WhenOrderIsNotShipped()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act & Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.MarkAsDelivered());
        }

        [Fact]
        public void Cancel_ShouldChangeStatusToCancelled()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.Cancel();

            // Assert
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void Cancel_ShouldThrowException_WhenOrderIsDelivered()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();
            order.MarkAsProcessing();
            order.MarkAsShipped();
            order.MarkAsDelivered();

            // Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.Cancel());
        }

        [Fact]
        public void Cancel_ShouldThrowException_WhenOrderIsShipped()
        {
             // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.MarkAsPaid();
            order.MarkAsProcessing();
            order.MarkAsShipped();

            // Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.Cancel());
        }

        [Fact]
        public void Cancel_ShouldThrowException_WhenOrderIsAlreadyCancelled()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act
            order.Cancel();

            // Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.Cancel());
        }

        [Fact]
        public void AddOrderItem_ShouldThrowProductOutOfStockException_WhenProductStockIsInsufficient()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            var product = new Product(
                Guid.NewGuid(),
                "Laptop",
                "Gaming laptop",
                "ART-001",
                "latop.jpg",
                new Money(1000m, "SEK"),
                1
            );
            var orderItem = new OrderItem(
                order.Id,
                product.Id,
                "Laptop",
                "ART-001",
                3,
                1000m
            );

            // Act & Assert
            Assert.Throws<ProductOutOfStockException>(() =>
            order.AddOrderItem(orderItem, product));
        }

        [Fact]
        public void MarkAsRefunded_ShouldSetPaymentStatusToRefunded_WhenOrderIsPaid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            order.MarkAsPaid();

            // Act
            order.MarkAsRefunded();

            // Assert
            Assert.Equal(PaymentStatus.Refunded, order.PaymentStatus);
            Assert.NotNull(order.UpdatedAt);
        }

        [Fact]
        public void MarkAsRefunded_ShouldThrowException_WhenOrderIsNotPaid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(
                customerId,
                "ORD-001",
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden"),
                new Address("Storgatan 1", "Stockholm", "11122", "Sweden")
            );

            // Act & Assert
            Assert.Throws<InvalidOrderStatusException>(() =>
            order.MarkAsRefunded());
        }
    }
}