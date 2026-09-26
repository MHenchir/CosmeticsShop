using CosmeticsShop.Application.Chatbot;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Ask(ChatRequest request, CancellationToken cancellationToken)
    {
        var response = await _chatService.AskAsync(request.CustomerId, request.Message, cancellationToken);
        return Ok(new ChatResponse(response));
    }
}

public sealed record ChatRequest(Guid CustomerId, string Message);
public sealed record ChatResponse(string Reply);