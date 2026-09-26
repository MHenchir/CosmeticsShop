using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Domain.Entities;

namespace CosmeticsShop.Application.Chatbot;

public sealed class ChatService
{
    private readonly IChatCompletionProvider _chatCompletionProvider;
    private readonly IChatConversationRepository _chatConversationRepository;

    private const string SystemPrompt =
        "Tu es l'assistant virtuel de CosmeticsShop, une boutique de cosmétiques. " +
        "Réponds de façon amicale et concise. Tu ne connais pas le catalogue précis " +
        "des produits pour l'instant — si le client demande un produit précis, dis-lui " +
        "de consulter le catalogue sur le site.";

    public ChatService(
        IChatCompletionProvider chatCompletionProvider,
        IChatConversationRepository chatConversationRepository)
    {
        _chatCompletionProvider = chatCompletionProvider;
        _chatConversationRepository = chatConversationRepository;
    }

    public async Task<string> AskAsync(Guid customerId, string userMessage, CancellationToken cancellationToken = default)
    {
        var conversation = await _chatConversationRepository.GetByCustomerIdAsync(customerId, cancellationToken);

        if (conversation is null)
        {
            conversation = new ChatConversation(customerId);
            conversation.AddMessage("system", SystemPrompt);
            await _chatConversationRepository.AddAsync(conversation, cancellationToken);
        }

        conversation.AddMessage("user", userMessage);

        var history = conversation.Messages
            .Select(m => new ChatMessage(m.Role, m.Content))
            .ToList();

        var response = await _chatCompletionProvider.GetCompletionAsync(history, cancellationToken);

        conversation.AddMessage("assistant", response);
        await _chatConversationRepository.SaveChangesAsync(cancellationToken);

        return response;
    }
}