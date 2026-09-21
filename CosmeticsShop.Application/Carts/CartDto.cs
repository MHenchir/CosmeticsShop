namespace CosmeticsShop.Application.Carts;

public sealed record CartDto(
    Guid Id,
    Guid CustomerId,
    IReadOnlyList<CartItemDto> Items,
    decimal Total,
    string Currency);