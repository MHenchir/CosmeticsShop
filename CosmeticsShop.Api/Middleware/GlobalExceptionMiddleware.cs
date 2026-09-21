using CosmeticsShop.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticsShop.Api.Middleware;

public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            // Erreur métier prévisible (Domain) → 400
            _logger.LogWarning(ex, "Violation de règle métier");
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Erreurs métier levées depuis Application (produit introuvable, email dupliqué...) → 400
            _logger.LogWarning(ex, "Opération invalide");
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            // Erreur technique imprévue → 500, message générique (jamais de détails techniques exposés)
            _logger.LogError(ex, "Erreur non gérée");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                "Une erreur inattendue est survenue.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, int statusCode, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Detail = detail
        });
    }
}