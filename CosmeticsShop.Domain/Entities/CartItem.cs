using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.Entities;

public sealed class CartItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private CartItem() { }

    internal CartItem(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        ProductId = productId;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("La quantité doit être positive.");

        Quantity += amount;
    }

    internal void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        Quantity = quantity;
    }
}