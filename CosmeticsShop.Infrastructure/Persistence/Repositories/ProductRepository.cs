using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsShop.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(
        string? searchTerm, decimal? maxPrice, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.Name.Contains(searchTerm));

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price.Amount <= maxPrice.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}