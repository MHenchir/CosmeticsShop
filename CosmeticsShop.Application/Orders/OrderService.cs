using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Orders;

public sealed class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    // Le client valide son panier → conversion en commande, prix figés ici
    public async Task<OrderDto> CheckoutAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);

        if (cart is null || cart.IsEmpty)
            throw new InvalidOperationException("Le panier est vide.");

        var order = new Order(customerId);

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException($"Produit {item.ProductId} introuvable.");

            if (!product.IsInStock(item.Quantity))
                throw new InvalidOperationException($"Stock insuffisant pour {product.Name}.");

            // Prix et nom COPIÉS ici — figés pour toujours dans la commande
            order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(order);
    }
    public async Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        return order is null ? null : MapToDto(order);
    }
    // Appelé après un paiement réussi (le futur IPaymentProvider, en Infrastructure)
    public async Task<OrderDto> ConfirmOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new InvalidOperationException("Commande introuvable.");

        order.Confirm();

        // Le stock n'est décrémenté QU'ICI, après confirmation du paiement
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            product?.DecreaseStock(item.Quantity);
        }

        await _orderRepository.SaveChangesAsync(cancellationToken);

        // Le panier est vidé une fois la commande confirmée
        var cart = await _cartRepository.GetByCustomerIdAsync(order.CustomerId, cancellationToken);
        cart?.Clear();
        await _cartRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(order);
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrderHistoryAsync(
        Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return orders.Select(MapToDto).ToList();
    }

    private static OrderDto MapToDto(Order order) => new(
        order.Id,
        order.CustomerId,
        order.OrderDate,
        order.Status.ToString(),
        order.Items.Select(i => new OrderItemDto(
            i.ProductId, i.ProductName, i.UnitPrice.Amount, i.UnitPrice.Currency, i.Quantity, i.LineTotal.Amount)).ToList(),
        order.TotalAmount.Amount,
        order.TotalAmount.Currency);
}