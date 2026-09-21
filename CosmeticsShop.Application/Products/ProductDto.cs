namespace CosmeticsShop.Application.Products;

// Ce que l'extérieur (Api, Angular) voit d'un produit — jamais l'entité Domain directement.
public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int StockQuantity,
    Guid CategoryId,
    string? CategoryName);