using System.Linq;
using CosmeticsShop.Domain.Entities;
using Xunit;

namespace CosmeticsShop.Tests.Domain;

public class CartTests
{
    [Fact]
    public void AddItem_SameProductTwice_MergesQuantity()
    {
        // Arrange
        var cart = new Cart(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        cart.AddItem(productId, 2);
        cart.AddItem(productId, 3);

        // Assert
        Assert.Single(cart.Items);
        Assert.Equal(5, cart.Items.First().Quantity);
    }

    [Fact]
    public void AddItem_DifferentProduct_CreatesSecondLine()
    {
        // Arrange
        var cart = new Cart(Guid.NewGuid());
        var serumId = Guid.NewGuid();
        var cremeId = Guid.NewGuid();

        // Act
        cart.AddItem(serumId, 2);
        cart.AddItem(cremeId, 1);

        // Assert
        Assert.Equal(2, cart.Items.Count);
    }
}