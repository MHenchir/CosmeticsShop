using CosmeticsShop.Application.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromToken();
        var dto = await _orderService.CheckoutAsync(customerId, cancellationToken);
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

    [HttpGet("history")]
    public async Task<IActionResult> GetOrderHistory(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromToken();
        var dtos = await _orderService.GetOrderHistoryAsync(customerId, cancellationToken);
        return Ok(dtos);
    }

    private Guid GetCustomerIdFromToken()
    {
        var customerIdClaim = User.FindFirst("customerId")?.Value
            ?? throw new InvalidOperationException("Token invalide : customerId manquant.");
        return Guid.Parse(customerIdClaim);
    }
}