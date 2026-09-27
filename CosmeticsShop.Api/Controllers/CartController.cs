using CosmeticsShop.Application.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromToken();
        var dto = await _cartService.AddToCartAsync(customerId, request.ProductId, request.Quantity, cancellationToken);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromToken();
        var dto = await _cartService.GetCartAsync(customerId, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveFromCart(Guid productId, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerIdFromToken();
        var dto = await _cartService.RemoveFromCartAsync(customerId, productId, cancellationToken);
        return Ok(dto);
    }

    private Guid GetCustomerIdFromToken()
    {
        var customerIdClaim = User.FindFirst("customerId")?.Value
            ?? throw new InvalidOperationException("Token invalide : customerId manquant.");
        return Guid.Parse(customerIdClaim);
    }
}

public sealed record AddToCartRequest(Guid ProductId, int Quantity);