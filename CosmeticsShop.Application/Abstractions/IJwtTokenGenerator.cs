namespace CosmeticsShop.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid customerId, string email);
}