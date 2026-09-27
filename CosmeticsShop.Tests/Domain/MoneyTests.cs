using CosmeticsShop.Domain.Exceptions;
using CosmeticsShop.Domain.ValueObjects;
using Xunit;

namespace CosmeticsShop.Tests.Domain;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Money(-10));
    }

    [Fact]
    public void Add_WithSameCurrency_ReturnsCorrectSum()
    {
        var a = new Money(10.50m);
        var b = new Money(5.25m);

        var result = a.Add(b);

        Assert.Equal(15.75m, result.Amount);
    }

    [Fact]
    public void Add_WithDifferentCurrencies_ThrowsDomainException()
    {
        var euros = new Money(10, "EUR");
        var dollars = new Money(10, "USD");

        Assert.Throws<DomainException>(() => euros.Add(dollars));
    }

    [Fact]
    public void Multiply_ReturnsCorrectProduct()
    {
        var price = new Money(15.90m);

        var result = price.Multiply(3);

        Assert.Equal(47.70m, result.Amount);
    }
}