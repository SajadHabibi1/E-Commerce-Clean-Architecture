using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? PhoneNumber { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private Customer() { }

        public Customer(string firstName, string lastName, string email, string? phoneNumber, Address? address)
        {
            var trimmedFirstName = firstName.Trim();
            var trimmedLastName = lastName.Trim();
            var trimmedEmail = email.Trim();

            EnsureValid(trimmedFirstName, trimmedLastName, trimmedEmail);

            Id = Guid.NewGuid();
            FirstName = trimmedFirstName;
            LastName = trimmedLastName;
            Email = trimmedEmail;
            PhoneNumber = phoneNumber?.Trim();
            Address = address;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }

        private static void EnsureValid(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException("FirstName cannot be empty");
            }
            
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("LastName cannot be empty");
            }
            
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("Email cannot be empty");
            }

            if (!email.Contains("@"))
            {
                throw new DomainException("Email must contain '@'");
            }
        }

        public void EnsureNotDeleted()
        {
            if (IsDeleted)
            {
                throw new DomainException("The customer is already deleted");
            }
        }

        public void Activate()
        {
            EnsureNotDeleted();

            if (IsActive)
            {
                throw new DomainException("Customer is already active");
            }

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            EnsureNotDeleted();

            if (!IsActive)
            {
                throw new DomainException("Customer is already inactive");
            }

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Edit(string firstName, string lastName, string email, string? phoneNumber, Address? address)
        {
            EnsureNotDeleted();

            var trimmedFirstName = firstName.Trim();
            var trimmedLastName = lastName.Trim();
            var trimmedEmail = email.Trim();

            EnsureValid(trimmedFirstName, trimmedLastName, trimmedEmail);

            FirstName = trimmedFirstName;
            LastName = trimmedLastName;
            Email = trimmedEmail;
            PhoneNumber = phoneNumber?.Trim();
            Address = address;

            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            EnsureNotDeleted();

            IsActive = false;
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            DeletedAt = DateTime.UtcNow;
        }
    }
}