using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.Entities;

public sealed class Cart
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart() { }

    public Cart(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
    }

    // Si le produit est déjà dans le panier, on augmente juste la quantité
    // plutôt que de créer une ligne en double.
    public void AddItem(Guid productId, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            _items.Add(new CartItem(productId, quantity));
        }
    }

    public void UpdateItemQuantity(Guid productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new DomainException("Ce produit n'est pas dans le panier.");

        item.SetQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new DomainException("Ce produit n'est pas dans le panier.");

        _items.Remove(item);
    }

    public void Clear() => _items.Clear();


    public bool IsEmpty => _items.Count == 0;
}