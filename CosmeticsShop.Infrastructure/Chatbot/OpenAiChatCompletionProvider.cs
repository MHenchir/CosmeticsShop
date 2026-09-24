using System.Text;
using System.Text.Json;
using CosmeticsShop.Application.Chatbot;
using Microsoft.Extensions.Configuration;

namespace CosmeticsShop.Infrastructure.Chatbot;

public sealed class OpenAiChatCompletionProvider : IChatCompletionProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string Model = "gpt-4o-mini";

    public OpenAiChatCompletionProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("Clé API OpenAI manquante (OpenAI:ApiKey).");
    }

    public async Task<string> GetCompletionAsync(
        IReadOnlyList<ChatMessage> conversationHistory,
        CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = Model,
            messages = conversationHistory.Select(m => new { role = m.Role, content = m.Content })
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = content
        };
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var document = JsonDocument.Parse(responseJson);

        var messageContent = document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return messageContent ?? "Désolé, je n'ai pas pu générer de réponse.";
    }
}