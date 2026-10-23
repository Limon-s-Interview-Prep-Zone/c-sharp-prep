#nullable enable
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Resource Filter (IResourceFilter)
/// Runs immediately after Authorization and BEFORE Model Binding occurs.
/// Wraps the entire rest of the action pipeline to measure full request processing latency.
/// </summary>
public class CustomResourceFilter : IResourceFilter
{
    private const string StopwatchItemKey = "__PipelineStopwatch";
    private readonly ILogger<CustomResourceFilter> _logger;

    public CustomResourceFilter(ILogger<CustomResourceFilter> logger)
    {
        _logger = logger;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        context.HttpContext.Items[StopwatchItemKey] = Stopwatch.StartNew();
        _logger.LogDebug("[ResourceFilter] Request entering MVC pipeline: {Path}", context.HttpContext.Request.Path);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue(StopwatchItemKey, out var swObj) && swObj is Stopwatch sw)
        {
            sw.Stop();
            var durationMs = sw.ElapsedMilliseconds;

            _logger.LogInformation("[ResourceFilter] MVC Pipeline duration for {Method} {Path}: {DurationMs} ms (Canceled: {Canceled})",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                durationMs,
                context.Canceled);
        }
    }
}

/// <summary>
/// 2. Production In-Memory Caching Resource Filter (IAsyncResourceFilter)
/// Short-circuits the pipeline BEFORE Model Binding on cache hit to eliminate CPU deserialization overhead.
/// Restricted to idempotent HTTP GET requests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AsyncCacheResourceFilter : Attribute, IAsyncResourceFilter
{
    private readonly int _durationSeconds;
    private IMemoryCache? _cache;
    private ILogger<AsyncCacheResourceFilter>? _logger;

    public AsyncCacheResourceFilter(int durationSeconds = 60)
    {
        _durationSeconds = durationSeconds;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // Resolve services on demand
        _cache ??= context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        _logger ??= context.HttpContext.RequestServices.GetRequiredService<ILogger<AsyncCacheResourceFilter>>();

        var request = context.HttpContext.Request;

        // Caching is strictly intended for idempotent HTTP GET requests
        if (!HttpMethods.IsGet(request.Method))
        {
            await next();
            return;
        }

        var normalizedPath = request.Path.Value?.ToLowerInvariant() ?? "/";
        var cacheKey = $"ResourceCache_{request.Method}_{normalizedPath}{request.QueryString.Value}";

        // 1. CACHE HIT: Short-circuit before model binding
        if (_cache.TryGetValue(cacheKey, out string? cachedBody) && !string.IsNullOrEmpty(cachedBody))
        {
            _logger.LogInformation("[ResourceCache] Cache HIT for key: {CacheKey}", cacheKey);

            if (!context.HttpContext.Response.HasStarted)
            {
                context.HttpContext.Response.Headers.TryAdd("X-Cache-Lookup", "HIT");
            }

            context.Result = new ContentResult
            {
                Content = cachedBody,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            return;
        }

        _logger.LogDebug("[ResourceCache] Cache MISS for key: {CacheKey}", cacheKey);

        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd("X-Cache-Lookup", "MISS");
        }

        // 2. Execute remainder of the pipeline (Model Binding -> Action -> Result)
        var executedContext = await next();

        // 3. Cache successful 200 OK responses
        if (executedContext.Result is ObjectResult objectResult &&
            objectResult.Value != null &&
            (objectResult.StatusCode == null || objectResult.StatusCode == StatusCodes.Status200OK))
        {
            var serialized = JsonSerializer.Serialize(objectResult.Value);
            _cache.Set(cacheKey, serialized, TimeSpan.FromSeconds(_durationSeconds));

            _logger.LogDebug("[ResourceCache] Cached response for {DurationSeconds}s with key: {CacheKey}",
                _durationSeconds, cacheKey);
        }
    }
}
