namespace CosmeticsShop.Application.Carts;

// Contrairement à CartItem (Domain), ce DTO inclut le nom et le prix ACTUEL
// du produit — c'est ici, en Application, qu'on va chercher ces infos live
// dans le catalogue au moment d'afficher le panier.
public sealed record CartItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    decimal LineTotal);