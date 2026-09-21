using CosmeticsShop.Domain.Enums;
using CosmeticsShop.Domain.Exceptions;
using CosmeticsShop.Domain.ValueObjects;

namespace CosmeticsShop.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Money TotalAmount => _items.Aggregate(Money.Zero, (total, item) => total.Add(item.LineTotal));

    private Order() { }

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        OrderDate = DateTimeOffset.UtcNow;
        Status = OrderStatus.Pending;
    }

    // On ne peut ajouter une ligne QUE via cette méthode — impossible de
    // manipuler _items depuis l'extérieur (il est private readonly).
    public void AddItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Impossible de modifier une commande déjà confirmée.");

        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
    }

    public void Confirm()
    {
        if (_items.Count == 0)
            throw new DomainException("Impossible de confirmer une commande vide.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new DomainException("Impossible d'annuler une commande déjà expédiée.");

        Status = OrderStatus.Cancelled;
    }
}