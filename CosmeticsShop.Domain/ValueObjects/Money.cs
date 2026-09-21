using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.ValueObjects;

// Value Object : pas d'identité propre, deux Money avec le même montant
// sont interchangeables. Immuable — impossible de changer le montant
// une fois créé, on ne peut qu'en créer un nouveau.
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "EUR")
    {
        if (amount < 0)
            throw new DomainException("Un montant ne peut pas être négatif.");

        Amount = amount;
        Currency = currency;
    }

    public static Money Zero => new(0);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Impossible d'additionner des montants de devises différentes.");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int factor) => new(Amount * factor, Currency);

    public override string ToString() => $"{Amount:0.00} {Currency}";
}