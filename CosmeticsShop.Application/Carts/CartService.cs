using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Carts;

public sealed class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> AddToCartAsync(
        Guid customerId, Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken)
            ?? throw new InvalidOperationException("Produit introuvable.");

        // Un seul panier actif par client : on le récupère, ou on en crée un nouveau
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        if (cart is null)
        {
            cart = new Cart(customerId);
            await _cartRepository.AddAsync(cart, cancellationToken);
        }

        cart.AddItem(productId, quantity);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        return await BuildCartDtoAsync(cart, cancellationToken);
    }

    public async Task<CartDto?> GetCartAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return cart is null ? null : await BuildCartDtoAsync(cart, cancellationToken);
    }

    public async Task<CartDto> RemoveFromCartAsync(
        Guid customerId, Guid productId, CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId, cancellationToken)
            ?? throw new InvalidOperationException("Panier introuvable.");

        cart.RemoveItem(productId);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        return await BuildCartDtoAsync(cart, cancellationToken);
    }

    // Le panier (Domain) ne connaît que ProductId + Quantity — c'est ICI qu'on
    // va chercher le prix et le nom ACTUELS de chaque produit pour construire le DTO complet.
    private async Task<CartDto> BuildCartDtoAsync(Cart cart, CancellationToken cancellationToken)
    {
        var itemDtos = new List<CartItemDto>();
        decimal total = 0;

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null) continue; // produit supprimé entre-temps : on l'ignore

            var lineTotal = product.Price.Amount * item.Quantity;
            total += lineTotal;

            itemDtos.Add(new CartItemDto(
                product.Id, product.Name, product.Price.Amount, product.Price.Currency,
                item.Quantity, lineTotal));
        }

        return new CartDto(cart.Id, cart.CustomerId, itemDtos, total, "EUR");
    }
}