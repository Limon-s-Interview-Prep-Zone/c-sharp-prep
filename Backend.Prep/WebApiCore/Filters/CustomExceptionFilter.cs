#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Exception Filter (IExceptionFilter)
/// Fallback error handler marking context.ExceptionHandled = true to prevent unhandled process crashes.
/// </summary>
public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var traceId = context.HttpContext.TraceIdentifier;
        _logger.LogError(context.Exception, "[CustomExceptionFilter] Unhandled exception occurred: {Message} (TraceId: {TraceId})",
            context.Exception.Message, traceId);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "An internal server error occurred while processing your request.",
            Instance = context.HttpContext.Request.Path,
            Type = "https://httpstatuses.com/500"
        };
        problemDetails.Extensions["traceId"] = traceId;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}

/// <summary>
/// 2. Asynchronous Enterprise API Exception Filter (IAsyncExceptionFilter)
/// Production-grade RFC-7807 ProblemDetails error translator:
/// - KeyNotFoundException -> 404 Not Found (LogWarning)
/// - ArgumentException -> 400 Bad Request (LogWarning)
/// - UnauthorizedAccessException -> 401 Unauthorized (LogWarning)
/// - InvalidOperationException / Other -> 500 Internal Server Error (LogError)
/// </summary>
public class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly IHostEnvironment _environment;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var traceId = context.HttpContext.TraceIdentifier;
        var path = context.HttpContext.Request.Path;

        int statusCode = context.Exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        // Differentiate log severity: 4xx are client warnings; 5xx are server errors
        if (statusCode >= 500)
        {
            _logger.LogError(context.Exception, "[ApiExceptionFilter] Server error {StatusCode} on {Path} (TraceId: {TraceId}): {Message}",
                statusCode, path, traceId, context.Exception.Message);
        }
        else
        {
            _logger.LogWarning("[ApiExceptionFilter] Client exception {StatusCode} on {Path} (TraceId: {TraceId}): {Message}",
                statusCode, path, traceId, context.Exception.Message);
        }

        // In Production, do not leak internal exception details for 500 errors
        string detail = (statusCode >= 500 && !_environment.IsDevelopment())
            ? "An unexpected internal server error occurred. Please contact support with the provided trace ID."
            : context.Exception.Message;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleForStatusCode(statusCode),
            Detail = detail,
            Instance = path,
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
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status404NotFound => "Not Found",
        _ => "Internal Server Error"
    };
}

/// <summary>
/// 3. Convenient Attribute for API Exception Handling
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiExceptionHandlingAttribute : TypeFilterAttribute
{
    public ApiExceptionHandlingAttribute() : base(typeof(ApiExceptionFilter))
    {
    }
}
