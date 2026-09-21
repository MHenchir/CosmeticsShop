using CosmeticsShop.Application.Carts;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/cart")]
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
        var dto = await _cartService.AddToCartAsync(
            request.CustomerId, request.ProductId, request.Quantity, cancellationToken);

        return Ok(dto);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCart(Guid customerId, CancellationToken cancellationToken)
    {
        var dto = await _cartService.GetCartAsync(customerId, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpDelete("{customerId:guid}/items/{productId:guid}")]
    public async Task<IActionResult> RemoveFromCart(Guid customerId, Guid productId, CancellationToken cancellationToken)
    {
        var dto = await _cartService.RemoveFromCartAsync(customerId, productId, cancellationToken);
        return Ok(dto);
    }
}

public sealed record AddToCartRequest(Guid CustomerId, Guid ProductId, int Quantity);