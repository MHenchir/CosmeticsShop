using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Auth;

public sealed class AuthService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        string firstName, string lastName, string email, string password, string? phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var existing = await _customerRepository.GetByEmailAsync(email, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("Un client avec cet email existe déjà.");

        var passwordHash = _passwordHasher.Hash(password);
        var customer = new Customer(firstName, lastName, email, passwordHash, phoneNumber);

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(customer.Id, customer.Email);
        return new AuthResponseDto(customer.Id, customer.Email, token);
    }

    public async Task<AuthResponseDto> LoginAsync(
        string email, string password, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new InvalidOperationException("Email ou mot de passe incorrect.");

        var isPasswordValid = _passwordHasher.Verify(password, customer.PasswordHash);
        if (!isPasswordValid)
            throw new InvalidOperationException("Email ou mot de passe incorrect.");

        var token = _jwtTokenGenerator.GenerateToken(customer.Id, customer.Email);
        return new AuthResponseDto(customer.Id, customer.Email, token);
    }
}