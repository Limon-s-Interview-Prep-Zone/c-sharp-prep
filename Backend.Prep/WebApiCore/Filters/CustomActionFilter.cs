#nullable enable
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Action Filter (IActionFilter)
/// Runs AFTER model binding has populated ActionArguments and ModelState.
/// Surrounds the execution of the action method itself.
/// </summary>
public class CustomActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        Console.WriteLine($"[CustomActionFilter] BEFORE Action Method: {actionName}");
        Console.WriteLine($"[CustomActionFilter] Action Arguments Count: {context.ActionArguments.Count}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        Console.WriteLine($"[CustomActionFilter] AFTER Action Method: {actionName}");

        if (context.Exception != null)
        {
            Console.WriteLine($"[CustomActionFilter] An unhandled exception was observed: {context.Exception.Message}");
            // ActionExecutedContext allows inspecting or modifying the returned result
        }
        else if (context.Result is ObjectResult objectResult)
        {
            Console.WriteLine($"[CustomActionFilter] Action returned ObjectResult with StatusCode: {objectResult.StatusCode ?? 200}");
        }
    }
}

/// <summary>
/// 2. Asynchronous Execution Timer Action Filter (IAsyncActionFilter)
/// Measures precise execution duration of the action method and appends
/// diagnostic response headers ('X-Action-Time-Ms').
/// </summary>
public class ExecutionTimerActionFilter : IAsyncActionFilter
{
    private readonly ILogger<ExecutionTimerActionFilter> _logger;

    public ExecutionTimerActionFilter(ILogger<ExecutionTimerActionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = context.ActionDescriptor.DisplayName ?? "UnknownAction";
        _logger.LogInformation("[ExecutionTimerActionFilter] Starting timer for action: {ActionName}", actionName);

        var stopwatch = Stopwatch.StartNew();

        // 1. Invoke the action method (and any inner filters)
        var executedContext = await next();

        // 2. Stop timer after action execution completes
        stopwatch.Stop();
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation("[ExecutionTimerActionFilter] Action {ActionName} completed in {ElapsedMs} ms", actionName, elapsedMs);

        // Add execution time to response headers (safe to add before ResultFilter flushes body)
        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd("X-Action-Time-Ms", elapsedMs.ToString());
        }
    }
}

/// <summary>
/// 3. Audit Logging Action Filter Attribute (ActionFilterAttribute)
/// Decorator attribute logging method entry, action parameters, and exit state for auditing.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuditLogAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
        var action = context.ActionDescriptor.DisplayName;

        Console.WriteLine($"[AuditLogAttribute] User '{user}' invoked '{action}'. Parameters:");
        foreach (var (key, value) in context.ActionArguments)
        {
            Console.WriteLine($"  - {key}: {value ?? "null"}");
        }
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var action = context.ActionDescriptor.DisplayName;
        var status = context.Exception == null ? "Success" : "Failed with Exception";
        Console.WriteLine($"[AuditLogAttribute] Action '{action}' completed with status: {status}");
    }
}

/// <summary>
/// 4. Action-level Caching Action Filter (ActionFilterAttribute)
/// Registered in DI or used as [ServiceFilter(typeof(CacheAsyncActionFilter))]
/// </summary>
public class CacheAsyncActionFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("[CacheAsyncActionFilter] BEFORE action method execution.");
        var resultContext = await next();
        Console.WriteLine("[CacheAsyncActionFilter] AFTER action method execution.");
    }
}
