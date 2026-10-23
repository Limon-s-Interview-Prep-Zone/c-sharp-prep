#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Result Filter (IResultFilter)
/// Runs immediately BEFORE and AFTER the action result is executed (e.g. JSON serialization).
/// Injects diagnostic tracing headers safely before response stream begins.
/// </summary>
public class CustomResultFilter : IResultFilter
{
    private readonly ILogger<CustomResultFilter> _logger;

    public CustomResultFilter(ILogger<CustomResultFilter> logger)
    {
        _logger = logger;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd("X-Correlation-ID", context.HttpContext.TraceIdentifier);
            context.HttpContext.Response.Headers.TryAdd("X-Processed-At", DateTime.UtcNow.ToString("o"));
            context.HttpContext.Response.Headers.TryAdd("X-Server-Name", Environment.MachineName);
        }

        _logger.LogDebug("[ResultFilter] Injected diagnostic headers for {Path}", context.HttpContext.Request.Path);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogDebug("[ResultFilter] Response body flushed for {Path} (Canceled: {Canceled})",
            context.HttpContext.Request.Path, context.Canceled);
    }
}

/// <summary>
/// 2. Asynchronous Result Filter (IAsyncResultFilter)
/// Demonstrates async result pipeline interception without blocking.
/// </summary>
public class AsyncResultFilter : IAsyncResultFilter
{
    private readonly ILogger<AsyncResultFilter> _logger;

    public AsyncResultFilter(ILogger<AsyncResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd("X-Filter-Pipeline", "AsyncResultFilter");
        }

        var executedContext = await next();

        _logger.LogDebug("[AsyncResultFilter] Completed serialization for {Path}", context.HttpContext.Request.Path);
    }
}

/// <summary>
/// 3. Standardized Envelope Result Filter (IAsyncResultFilter)
/// Wraps successful action results into a standardized enterprise API envelope:
/// { "success": true, "data": ..., "traceId": ..., "timestamp": ... }
/// Skips wrapping on ProblemDetails, empty results, or already wrapped responses.
/// </summary>
public class StandardizedResponseEnvelopeFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Only wrap successful ObjectResults (e.g., 200 OK) that are not ProblemDetails
        if (context.Result is ObjectResult objectResult &&
            objectResult.Value != null &&
            objectResult.Value is not ProblemDetails &&
            (objectResult.StatusCode == null || (objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)))
        {
            var traceId = context.HttpContext.TraceIdentifier;
            var valueType = objectResult.Value.GetType();

            if (!valueType.Name.StartsWith("ApiResponse"))
            {
                objectResult.Value = new
                {
                    Success = true,
                    Data = objectResult.Value,
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        await next();
    }
}

/// <summary>
/// 4. Custom Response Header Attribute (ResultFilterAttribute)
/// Usable directly as [ResponseHeader("X-Custom-Header", "Value")] on controllers or actions.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ResponseHeaderAttribute : ResultFilterAttribute
{
    private readonly string _name;
    private readonly string _value;

    public ResponseHeaderAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd(_name, _value);
        }
    }
}
