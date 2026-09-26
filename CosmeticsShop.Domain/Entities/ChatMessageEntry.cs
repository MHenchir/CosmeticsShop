namespace CosmeticsShop.Domain.Entities;

public sealed class ChatMessageEntry
{
    public Guid Id { get; private set; }
    public string Role { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset SentAt { get; private set; }

    private ChatMessageEntry() { }

    internal ChatMessageEntry(string role, string content)
    {
        Id = Guid.NewGuid();
        Role = role;
        Content = content;
        SentAt = DateTimeOffset.UtcNow;
    }
}