using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ResolveBridge.Application.Common;

namespace ResolveBridge.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "The requested resource was not found"),
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid input provided"),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message), // User friendly if it's a domain logic error
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred on the server. Please try again later.")
        };

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = ApiResponse<object>.Failed(message, new List<string> { exception.Message });

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
