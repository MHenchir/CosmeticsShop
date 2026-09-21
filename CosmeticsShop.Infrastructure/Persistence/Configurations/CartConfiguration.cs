using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticsShop.Infrastructure.Persistence.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerId).IsRequired();

        // Un seul panier actif par client (décidé ensemble plus tôt)
        builder.HasIndex(c => c.CustomerId).IsUnique();

        builder.Ignore(c => c.IsEmpty); // calculé, pas stocké

        builder.Navigation(c => c.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(c => c.Items, item =>
        {
            item.ToTable("CartItems");
            item.WithOwner().HasForeignKey("CartId");

            // CartItem n'a PAS d'Id propre (contrairement à OrderItem) :
            // sa clé est la combinaison (CartId + ProductId)
            item.HasKey("CartId", nameof(CartItem.ProductId));

            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.Quantity).IsRequired();
        });
    }
}