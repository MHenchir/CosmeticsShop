using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Abstractions;

public interface IChatConversationRepository
{
    Task<ChatConversation?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task AddAsync(ChatConversation conversation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    // Utilisé plus tard par le nettoyage périodique (étape 9)
    Task<IReadOnlyList<ChatConversation>> GetInactiveConversationsAsync(
        DateTimeOffset olderThan, CancellationToken cancellationToken = default);
    Task RemoveRangeAsync(IReadOnlyList<ChatConversation> conversations, CancellationToken cancellationToken = default);
}