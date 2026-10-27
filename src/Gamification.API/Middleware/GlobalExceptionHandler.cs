using Gamification.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Middleware;

/// <summary>
/// Global exception handling middleware.
/// Translates application exceptions to ProblemDetails responses:
/// UnauthorizedException → 401 Unauthorized.
/// Unexpected exceptions remain unchanged so they surface as HTTP 500.
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware, translating known application exceptions.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedException ex)
        {
            // Invalid or missing credentials always result in 401 Unauthorized
            _logger.LogWarning(ex, "Access denied for {Path}.", context.Request.Path);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Access denied.",
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(
                problemDetails,
                options: null,
                contentType: "application/problem+json");
        }
    }
}
