using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsShop.Infrastructure.Persistence.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly AppDbContext _dbContext;

    public CartRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _dbContext.Carts.AddAsync(cart, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}