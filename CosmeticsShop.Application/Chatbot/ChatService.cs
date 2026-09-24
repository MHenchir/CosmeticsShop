namespace CosmeticsShop.Application.Chatbot;

public sealed class ChatService
{
    private readonly IChatCompletionProvider _chatCompletionProvider;

    private const string SystemPrompt =
        "Tu es l'assistant virtuel de CosmeticsShop, une boutique de cosmétiques. " +
        "Réponds de façon amicale et concise. Tu ne connais pas le catalogue précis " +
        "des produits pour l'instant — si le client demande un produit précis, dis-lui " +
        "de consulter le catalogue sur le site.";

    public ChatService(IChatCompletionProvider chatCompletionProvider)
    {
        _chatCompletionProvider = chatCompletionProvider;
    }

    public async Task<string> AskAsync(string userMessage, CancellationToken cancellationToken = default)
    {
        var conversation = new List<ChatMessage>
        {
            new("system", SystemPrompt),
            new("user", userMessage)
        };

        return await _chatCompletionProvider.GetCompletionAsync(conversation, cancellationToken);
    }
}