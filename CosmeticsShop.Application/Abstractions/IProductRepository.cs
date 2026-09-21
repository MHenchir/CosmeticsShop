using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Abstractions;

// Le "port" — Application définit ce dont elle a besoin, sans savoir
// comment c'est fait techniquement (SQL Server, EF Core, etc. viendront
// dans Infrastructure, qui implémentera cette interface).
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> SearchAsync(string? searchTerm, decimal? maxPrice, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}