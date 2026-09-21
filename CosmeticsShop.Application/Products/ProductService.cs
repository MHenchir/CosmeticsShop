using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;
using CosmeticsShop.Domain.ValueObjects;

namespace CosmeticsShop.Application.Products;

public sealed class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : MapToDto(product);
    }

    public async Task<IReadOnlyList<ProductDto>> SearchProductsAsync(
        string? searchTerm, decimal? maxPrice, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.SearchAsync(searchTerm, maxPrice, cancellationToken);
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto> CreateProductAsync(
        string name, string description, decimal price, int initialStock, Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var product = new Product(name, description, new Money(price), initialStock, categoryId);

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(product);
    }

    private static ProductDto MapToDto(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price.Amount,
        product.Price.Currency,
        product.StockQuantity,
        product.CategoryId,
        product.Category?.Name);
}