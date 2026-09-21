using CosmeticsShop.Domain.ValueObjects;

namespace CosmeticsShop.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } // copie figée au moment de la commande
    public Money UnitPrice { get; private set; }      // copie figée : le prix produit peut changer après
    public int Quantity { get; private set; }

    public Money LineTotal => UnitPrice.Multiply(Quantity);

    private OrderItem() { }

    internal OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}