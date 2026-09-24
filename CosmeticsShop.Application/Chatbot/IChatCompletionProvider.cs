namespace CosmeticsShop.Application.Chatbot;

// Le port — Application dit CE dont elle a besoin (envoyer des messages,
// recevoir une réponse), sans savoir COMMENT c'est fait techniquement.
public interface IChatCompletionProvider
{
    Task<string> GetCompletionAsync(
        IReadOnlyList<ChatMessage> conversationHistory,
        CancellationToken cancellationToken = default);
}

public sealed record ChatMessage(string Role, string Content);
// Role : "system" (instructions), "user" (le client), "assistant" (le chatbot)