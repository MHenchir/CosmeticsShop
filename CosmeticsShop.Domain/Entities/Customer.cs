using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; }

    private Customer() { }

    public Customer(string firstName, string lastName, string email, string passwordHash, string? phoneNumber = null)
    {
        if (!email.Contains('@'))
            throw new DomainException("L'adresse email n'est pas valide.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Le mot de passe est obligatoire.");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
    }

    public string FullName => $"{FirstName} {LastName}";
}