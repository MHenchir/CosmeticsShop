using CosmeticsShop.Domain.Entities;
using CosmeticsShop.Domain.Enums;
using CosmeticsShop.Domain.Exceptions;
using CosmeticsShop.Domain.ValueObjects;
using Xunit;

namespace CosmeticsShop.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Constructor_CreatesOrderWithPendingStatus()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void AddItem_ToNewOrder_AddsItemCorrectly()
    {
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        order.AddItem(productId, "Sérum vitamine C", new Money(24.90m), 3);

        Assert.Single(order.Items);
        Assert.Equal(74.70m, order.TotalAmount.Amount);
    }

    [Fact]
    public void AddItem_WithMultipleLines_CalculatesTotalCorrectly()
    {
        var order = new Order(Guid.NewGuid());

        order.AddItem(Guid.NewGuid(), "Sérum", new Money(24.90m), 3);
        order.AddItem(Guid.NewGuid(), "Crème", new Money(18.50m), 1);

        Assert.Equal(2, order.Items.Count);
        Assert.Equal(93.20m, order.TotalAmount.Amount);
    }

    [Fact]
    public void AddItem_WithNegativeQuantity_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            order.AddItem(Guid.NewGuid(), "Sérum", new Money(24.90m), -1));
    }

    [Fact]
    public void AddItem_AfterConfirm_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), "Sérum", new Money(24.90m), 1);
        order.Confirm();

        Assert.Throws<DomainException>(() =>
            order.AddItem(Guid.NewGuid(), "Crème", new Money(18.50m), 1));
    }

    [Fact]
    public void Confirm_WithEmptyOrder_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() => order.Confirm());
    }

    [Fact]
    public void Confirm_WithItems_ChangesStatusToConfirmed()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), "Sérum", new Money(24.90m), 1);

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Cancel_PendingOrder_ChangesStatusToCancelled()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), "Sérum", new Money(24.90m), 1);

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }
}