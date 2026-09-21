namespace CosmeticsShop.Application.Orders;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    DateTimeOffset OrderDate,
    string Status,
    IReadOnlyList<OrderItemDto> Items,
    decimal Total,
    string Currency);