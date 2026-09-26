using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsShop.Infrastructure.Persistence.Repositories;

public sealed class ChatConversationRepository : IChatConversationRepository
{
    private readonly AppDbContext _dbContext;

    public ChatConversationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChatConversation?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatConversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(ChatConversation conversation, CancellationToken cancellationToken = default)
    {
        await _dbContext.ChatConversations.AddAsync(conversation, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ChatConversation>> GetInactiveConversationsAsync(
        DateTimeOffset olderThan, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatConversations
            .Where(c => c.LastMessageAt < olderThan)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync(IReadOnlyList<ChatConversation> conversations, CancellationToken cancellationToken = default)
    {
        _dbContext.ChatConversations.RemoveRange(conversations);
        await Task.CompletedTask;
    }
}