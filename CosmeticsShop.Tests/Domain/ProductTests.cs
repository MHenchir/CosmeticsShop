using CosmeticsShop.Domain.Entities;
using CosmeticsShop.Domain.Exceptions;
using CosmeticsShop.Domain.ValueObjects;
using Xunit;

namespace CosmeticsShop.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_WithEmptyName_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
            new Product("", "description", new Money(10), 5, Guid.NewGuid()));
    }

    [Fact]
    public void DecreaseStock_WithSufficientStock_DecreasesQuantity()
    {
        var product = new Product("Sérum", "description", new Money(24.90m), 15, Guid.NewGuid());

        product.DecreaseStock(4);

        Assert.Equal(11, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_WithInsufficientStock_ThrowsDomainException()
    {
        var product = new Product("Sérum", "description", new Money(24.90m), 3, Guid.NewGuid());

        Assert.Throws<DomainException>(() => product.DecreaseStock(10));
    }

    [Fact]
    public void DecreaseStock_WithNegativeQuantity_ThrowsDomainException()
    {
        var product = new Product("Sérum", "description", new Money(24.90m), 10, Guid.NewGuid());

        Assert.Throws<DomainException>(() => product.DecreaseStock(-1));
    }

    [Fact]
    public void IsInStock_WithSufficientQuantity_ReturnsTrue()
    {
        var product = new Product("Sérum", "description", new Money(24.90m), 10, Guid.NewGuid());

        var result = product.IsInStock(5);

        Assert.True(result);
    }

    [Fact]
    public void IsInStock_WithInsufficientQuantity_ReturnsFalse()
    {
        var product = new Product("Sérum", "description", new Money(24.90m), 3, Guid.NewGuid());

        var result = product.IsInStock(5);

        Assert.False(result);
    }
}