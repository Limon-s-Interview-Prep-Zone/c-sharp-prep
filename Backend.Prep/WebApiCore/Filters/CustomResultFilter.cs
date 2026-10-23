#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Result Filter (IResultFilter)
/// Runs immediately BEFORE and AFTER the action result is executed (e.g. JSON serialization).
/// CRITICAL: HTTP headers MUST be set in OnResultExecuting, BEFORE response streaming begins.
/// </summary>
public class CustomResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("[CustomResultFilter] OnResultExecuting: Adding diagnostic response headers...");

        // Safe to add headers here because response body serialization hasn't started yet
        context.HttpContext.Response.Headers.TryAdd("X-Correlation-ID", Guid.NewGuid().ToString("N"));
        context.HttpContext.Response.Headers.TryAdd("X-Processed-At", DateTime.UtcNow.ToString("o"));
        context.HttpContext.Response.Headers.TryAdd("X-Server-Name", Environment.MachineName);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        // Response headers are READ-ONLY at this stage; response body has already been flushed to the socket
        Console.WriteLine($"[CustomResultFilter] OnResultExecuted: Response generation completed. Canceled: {context.Canceled}");
    }
}

/// <summary>
/// 2. Asynchronous Result Filter (IAsyncResultFilter)
/// Demonstrates async result execution pipeline interception.
/// </summary>
public class AsyncResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        Console.WriteLine("[AsyncResultFilter] BEFORE IActionResult execution (Pre-serialization)...");

        // Set response header before serialization begins
        context.HttpContext.Response.Headers.TryAdd("X-Filter-Type", "AsyncResultFilter");

        // Execute the result (serializes JSON to HTTP response body)
        var executedContext = await next();

        Console.WriteLine("[AsyncResultFilter] AFTER IActionResult execution (Post-serialization).");
    }
}

/// <summary>
/// 3. Standardized Envelope Result Filter (IAsyncResultFilter)
/// Wraps raw action result objects into a standardized enterprise API envelope:
/// { "success": true, "data": ..., "traceId": ... }
/// </summary>
public class StandardizedResponseEnvelopeFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Inspect the action result before serialization
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var traceId = context.HttpContext.TraceIdentifier;

            // Avoid double wrapping if already an envelope
            var valueType = objectResult.Value.GetType();
            if (!valueType.Name.StartsWith("ApiResponse"))
            {
                objectResult.Value = new
                {
                    Success = objectResult.StatusCode is null or >= 200 and < 300,
                    Data = objectResult.Value,
                    TraceId = traceId,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        // Proceed to serialize wrapped response
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
        context.HttpContext.Response.Headers.TryAdd(_name, _value);
    }
}
