using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticsShop.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        // Index UNIQUE : empêche 2 clients d'avoir le même email,
        // garanti au niveau base de données (pas seulement dans le code)
        builder.HasIndex(c => c.Email).IsUnique();
    }
}