using CosmeticsShop.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var dto = await _productService.CreateProductAsync(
            request.Name, request.Description, request.Price, request.Stock, request.CategoryId, cancellationToken);

        return CreatedAtAction(nameof(GetProduct), new { id = dto.Id }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _productService.GetProductAsync(id, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string? searchTerm, [FromQuery] decimal? maxPrice, CancellationToken cancellationToken)
    {
        var products = await _productService.SearchProductsAsync(searchTerm, maxPrice, cancellationToken);
        return Ok(products);
    }
}

public sealed record CreateProductRequest(
    string Name, string Description, decimal Price, int Stock, Guid CategoryId);