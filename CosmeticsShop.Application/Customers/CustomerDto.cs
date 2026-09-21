namespace CosmeticsShop.Application.Customers;

public sealed record CustomerDto(
    Guid Id, string FirstName, string LastName, string Email, string? PhoneNumber);