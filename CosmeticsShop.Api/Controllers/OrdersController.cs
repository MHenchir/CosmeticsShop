using CosmeticsShop.Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutRequest request, CancellationToken cancellationToken)
    {
        var dto = await _orderService.CheckoutAsync(request.CustomerId, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { orderId = dto.Id }, dto);
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var dto = await _orderService.ConfirmOrderAsync(orderId, cancellationToken);
        return Ok(dto);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var dto = await _orderService.GetOrderAsync(orderId, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetOrderHistory(Guid customerId, CancellationToken cancellationToken)
    {
        var dtos = await _orderService.GetOrderHistoryAsync(customerId, cancellationToken);
        return Ok(dtos);
    }
}

public sealed record CheckoutRequest(Guid CustomerId);