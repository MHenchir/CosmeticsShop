using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    private Customer() { }

    public Customer(string firstName, string lastName, string email, string? phoneNumber = null)
    {
        if (!email.Contains('@'))
            throw new DomainException("L'adresse email n'est pas valide.");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string FullName => $"{FirstName} {LastName}";
}