using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticsShop.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId).IsRequired();
        builder.Property(o => o.OrderDate).IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Ignore(o => o.TotalAmount);

        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(o => o.Items, item =>
        {
            item.ToTable("OrderItems");
            item.WithOwner().HasForeignKey("OrderId");
            item.HasKey(i => i.Id);

            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            item.Property(i => i.Quantity).IsRequired();

            item.Ignore(i => i.LineTotal);

            item.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("UnitPriceAmount")
                    .HasColumnType("decimal(18,2)");

                price.Property(m => m.Currency)
                    .HasColumnName("UnitPriceCurrency")
                    .HasMaxLength(3);
            });
        });

        builder.HasIndex(o => o.CustomerId);
    }
}