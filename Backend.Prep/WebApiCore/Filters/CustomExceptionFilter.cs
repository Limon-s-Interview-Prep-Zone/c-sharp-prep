#nullable enable
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Exception Filter (IExceptionFilter)
/// Catches unhandled exceptions thrown by Controller Actions and Action Filters.
/// Crucial: setting context.ExceptionHandled = true notifies ASP.NET Core that the error has been processed.
/// </summary>
public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Console.WriteLine($"[CustomExceptionFilter] Handled exception: {context.Exception.GetType().Name} - {context.Exception.Message}");

        var errorResponse = new
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "A system error occurred while processing your request.",
            Detail = context.Exception.Message,
            TraceId = context.HttpContext.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        // Construct response and mark handled
        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true; // Prevents the exception from bubbling to global middleware
    }
}

/// <summary>
/// 2. Asynchronous Enterprise API Exception Filter (IAsyncExceptionFilter)
/// Formats unhandled exceptions into RFC-7807 ProblemDetails with specific HTTP status codes:
/// - KeyNotFoundException -> 404 Not Found
/// - ArgumentException -> 400 Bad Request
/// - UnauthorizedAccessException -> 401 Unauthorized
/// - InvalidOperationException / Default -> 500 Internal Server Error
/// </summary>
public class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var traceId = context.HttpContext.TraceIdentifier;
        _logger.LogError(context.Exception, "[ApiExceptionFilter] Unhandled exception in {Path} (TraceId: {TraceId}): {Message}",
            context.HttpContext.Request.Path, traceId, context.Exception.Message);

        int statusCode = context.Exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleForStatusCode(statusCode),
            Detail = context.Exception.Message,
            Instance = context.HttpContext.Request.Path,
            Type = $"https://httpstatuses.com/{statusCode}"
        };

        problemDetails.Extensions["traceId"] = traceId;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }

    private static string GetTitleForStatusCode(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request - Invalid Parameters",
        StatusCodes.Status401Unauthorized => "Unauthorized Access",
        StatusCodes.Status404NotFound => "Requested Resource Not Found",
        _ => "Internal Server Error"
    };
}

/// <summary>
/// 3. Convenient Attribute for API Exception Handling
/// Allows applying the ApiExceptionFilter to specific controllers or actions: [ApiExceptionHandling]
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiExceptionHandlingAttribute : TypeFilterAttribute
{
    public ApiExceptionHandlingAttribute() : base(typeof(ApiExceptionFilter))
    {
    }
}
