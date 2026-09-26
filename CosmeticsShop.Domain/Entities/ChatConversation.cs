using CosmeticsShop.Domain.Exceptions;

namespace CosmeticsShop.Domain.Entities;

public sealed class ChatConversation
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTimeOffset LastMessageAt { get; private set; }

    private readonly List<ChatMessageEntry> _messages = new();
    public IReadOnlyCollection<ChatMessageEntry> Messages => _messages.AsReadOnly();

    private ChatConversation() { }

    public ChatConversation(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        LastMessageAt = DateTimeOffset.UtcNow;
    }

    public void AddMessage(string role, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Le contenu du message ne peut pas être vide.");

        _messages.Add(new ChatMessageEntry(role, content));
        LastMessageAt = DateTimeOffset.UtcNow;
    }
}