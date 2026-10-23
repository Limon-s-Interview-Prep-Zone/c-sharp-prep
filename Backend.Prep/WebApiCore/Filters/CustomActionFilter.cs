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
    private readonly ILogger<CustomActionFilter> _logger;

    public CustomActionFilter(ILogger<CustomActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogDebug("[ActionFilter] BEFORE executing action: {ActionName} with {ArgCount} argument(s)",
            actionName, context.ActionArguments.Count);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;

        if (context.Exception != null)
        {
            _logger.LogWarning("[ActionFilter] AFTER action: {ActionName} resulted in exception: {ErrorMessage}",
                actionName, context.Exception.Message);
        }
        else
        {
            var statusCode = (context.Result as ObjectResult)?.StatusCode ?? StatusCodes.Status200OK;
            _logger.LogDebug("[ActionFilter] AFTER action: {ActionName} completed with StatusCode {StatusCode}",
                actionName, statusCode);
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
        var stopwatch = Stopwatch.StartNew();

        // Execute action method and inner filters
        var executedContext = await next();

        stopwatch.Stop();
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation("[ActionTimer] Action '{ActionName}' completed in {ElapsedMs} ms", actionName, elapsedMs);

        // Safe to add headers before response body streaming begins
        if (!context.HttpContext.Response.HasStarted)
        {
            context.HttpContext.Response.Headers.TryAdd("X-Action-Time-Ms", elapsedMs.ToString());
        }
    }
}

/// <summary>
/// 3. Audit Logging Action Filter Attribute (ActionFilterAttribute)
/// Production audit decorator logging user identity, action descriptor, and parameter summary.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuditLogAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<AuditLogAttribute>>();
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
        var action = context.ActionDescriptor.DisplayName;
        var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        logger?.LogInformation("[AuditLog] User '{User}' from IP '{ClientIp}' executing '{Action}' with {ArgCount} parameter(s)",
            user, clientIp, action, context.ActionArguments.Count);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<AuditLogAttribute>>();
        var action = context.ActionDescriptor.DisplayName;
        var status = context.Exception == null ? "Success" : "Failed with Exception";

        logger?.LogInformation("[AuditLog] Action '{Action}' completed with status: {Status}", action, status);
    }
}

/// <summary>
/// 4. Action-level Execution Hook (ActionFilterAttribute)
/// Registered in DI or applied via [ServiceFilter(typeof(CacheAsyncActionFilter))]
/// </summary>
public class CacheAsyncActionFilter : ActionFilterAttribute
{
    private readonly ILogger<CacheAsyncActionFilter> _logger;

    public CacheAsyncActionFilter(ILogger<CacheAsyncActionFilter> logger)
    {
        _logger = logger;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogDebug("[ServiceFilterAction] Hook before action: {Action}", context.ActionDescriptor.DisplayName);
        var resultContext = await next();
        _logger.LogDebug("[ServiceFilterAction] Hook after action: {Action}", context.ActionDescriptor.DisplayName);
    }
}
