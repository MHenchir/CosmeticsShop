namespace CosmeticsShop.Application.Auth;

public sealed record AuthResponseDto(Guid CustomerId, string Email, string Token);