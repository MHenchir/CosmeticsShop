namespace CosmeticsShop.Domain.Exceptions;

// Toute violation d'une règle métier lève cette exception —
// jamais une exception technique générique (ArgumentException, etc.)
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}