#nullable enable
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Resource Filter (IResourceFilter)
/// Runs immediately after Authorization and BEFORE Model Binding occurs.
/// Wraps the entire rest of the action pipeline (model binding, action, exception, result).
/// </summary>
public class CustomResourceFilter : IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine("[CustomResourceFilter] OnResourceExecuting: BEFORE Model Binding.");
        // We can inspect raw HTTP headers or query strings before any JSON serialization/model binding begins
        context.HttpContext.Items["PipelineStartTime"] = DateTime.UtcNow;
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine("[CustomResourceFilter] OnResourceExecuted: Pipeline finished rendering.");
        if (context.HttpContext.Items.TryGetValue("PipelineStartTime", out var startTimeObj) && startTimeObj is DateTime startTime)
        {
            var totalPipelineDuration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            Console.WriteLine($"[CustomResourceFilter] Total MVC Pipeline Duration: {totalPipelineDuration:F2} ms");
        }
    }
}

/// <summary>
/// 2. Asynchronous In-Memory Caching Resource Filter (IAsyncResourceFilter)
/// Demonstrates the primary architectural benefit of Resource Filters:
/// Short-circuiting BEFORE model binding to serve cached responses instantly with ZERO model binding CPU cost.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AsyncCacheResourceFilter : Attribute, IAsyncResourceFilter
{
    private readonly int _durationSeconds;
    private IMemoryCache? _cache;

    public AsyncCacheResourceFilter(int durationSeconds = 60)
    {
        _durationSeconds = durationSeconds;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // Resolve IMemoryCache from the request services
        _cache ??= context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

        var cacheKey = $"Cache_{context.HttpContext.Request.Method}_{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";

        Console.WriteLine($"[AsyncCacheResourceFilter] Checking cache for key: {cacheKey}");

        // 1. CACHE HIT: Short-circuit immediately BEFORE model binding
        if (_cache.TryGetValue(cacheKey, out string? cachedResponse) && !string.IsNullOrEmpty(cachedResponse))
        {
            Console.WriteLine("[AsyncCacheResourceFilter] Cache HIT! Short-circuiting pipeline with cached response.");
            context.Result = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            return; // Short-circuit: skips Model Binding, Action Filters, and Action Method!
        }

        Console.WriteLine("[AsyncCacheResourceFilter] Cache MISS. Proceeding down the pipeline...");

        // 2. EXECUTE REMAINDER OF PIPELINE (Model Binding -> Action -> Result)
        var executedContext = await next();

        // 3. CACHE POPULATION: Cache successful 200 OK responses
        if (executedContext.Result is ObjectResult objectResult &&
            objectResult.Value is not null &&
            (objectResult.StatusCode == null || objectResult.StatusCode == StatusCodes.Status200OK))
        {
            var jsonResponse = JsonSerializer.Serialize(objectResult.Value);
            _cache.Set(cacheKey, jsonResponse, TimeSpan.FromSeconds(_durationSeconds));
            Console.WriteLine($"[AsyncCacheResourceFilter] Response cached for {_durationSeconds} seconds.");
        }
    }
}
