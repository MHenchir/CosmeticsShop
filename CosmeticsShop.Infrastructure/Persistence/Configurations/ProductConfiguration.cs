using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosmeticsShop.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        // Money est un Value Object (pas une entité) : on l'"éclate" en 2 colonnes
        // directement dans la table Products, avec OwnsOne.
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(m => m.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)");

            price.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3);
        });

        // Relation vers Category : un produit appartient à une catégorie
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // empêche de supprimer une catégorie qui a des produits

        // L'index dont on avait parlé — accélère la recherche par nom
        builder.HasIndex(p => p.Name);

        // Accélère le filtrage par catégorie
        builder.HasIndex(p => p.CategoryId);
    }
}