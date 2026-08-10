using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

            builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

            builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20);

            builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street)
                .HasColumnName("Street");

                address.Property(a => a.City)
                .HasColumnName("City");

                address.Property(a => a.PostalCode)
                .HasColumnName("PostalCode");

                address.Property(a => a.Country)
                .HasColumnName("Country");
            });
        }
    }
}