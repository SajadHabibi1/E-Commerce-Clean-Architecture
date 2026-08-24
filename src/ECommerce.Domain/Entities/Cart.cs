using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private readonly List<CartItem> _cartItems = new();
        public IReadOnlyList<CartItem> CartItems => _cartItems;

        public Cart (Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;

            EnsureValid();
        }

        private void EnsureValid()
        {
            if (CustomerId == Guid.Empty)
            {
                throw new DomainException("CustomerId is required");
            }
        }

        private void CalculateAmount()
        {
            TotalAmount = _cartItems.Sum(item => item.Quantity * item.UnitPrice);
        }

        public void AddItem(CartItem item)
        {
            if (item == null)
            {
                throw new DomainException("CartItem cannot be null");
            }

            _cartItems.Add(item);
            CalculateAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveItem(Guid cartItemId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == cartItemId);

            if (item == null)
            {
                throw new DomainException("CartItem not found");
            }

            _cartItems.Remove(item);
            CalculateAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeItemQuantity(Guid cartItemId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == cartItemId);

            if (item == null)
            {
                throw new DomainException("CartItem not found");
            }
            item.ChangeQuantity(quantity);

            CalculateAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void Clear()
        {
            _cartItems.Clear();
            CalculateAmount();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}