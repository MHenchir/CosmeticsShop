using CosmeticsShop.Domain.Exceptions;
using CosmeticsShop.Domain.ValueObjects;

namespace CosmeticsShop.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private Product() { } // requis par EF Core

    public Product(string name, string description, Money price, int initialStock, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom du produit est obligatoire.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = initialStock;
        CategoryId = categoryId;
    }

    // Le stock ne peut être modifié QUE via ces méthodes — jamais en accès
    // direct depuis l'extérieur. C'est ça, l'encapsulation d'un domaine riche.
    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        if (StockQuantity < quantity)
            throw new DomainException($"Stock insuffisant pour {Name} (disponible : {StockQuantity}, demandé : {quantity}).");

        StockQuantity -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        StockQuantity += quantity;
    }

    public bool IsInStock(int requestedQuantity) => StockQuantity >= requestedQuantity;
}