#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Validation Filter (IActionFilter)
/// Checks ModelState.IsValid and short-circuits with RFC 7807 ValidationProblemDetails before the action runs.
/// </summary>
public class ValidationActionFilter : IActionFilter
{
    private readonly ILogger<ValidationActionFilter> _logger;

    public ValidationActionFilter(ILogger<ValidationActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            _logger.LogWarning("[ValidationFilter] Model validation failed on {Path}. Short-circuiting pipeline.",
                context.HttpContext.Request.Path);

            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

/// <summary>
/// 2. Asynchronous Validation Filter (IAsyncActionFilter)
/// Idiomatic modern .NET async pattern for non-blocking model validation.
/// </summary>
public class AsyncValidationActionFilter : IAsyncActionFilter
{
    private readonly ILogger<AsyncValidationActionFilter> _logger;

    public AsyncValidationActionFilter(ILogger<AsyncValidationActionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            _logger.LogWarning("[AsyncValidationFilter] Model validation failed on {Path}.", context.HttpContext.Request.Path);

            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation failure",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new BadRequestObjectResult(problemDetails);
            return; // Short-circuit: do not call next()
        }

        await next();
    }
}

/// <summary>
/// 3. Attribute-Based Model Validation Filter (ActionFilterAttribute)
/// Usable directly as [ValidateModel] on specific controllers or action methods.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<ValidateModelAttribute>>();
            logger?.LogWarning("[ValidateModel] Validation error on {Path}", context.HttpContext.Request.Path);

            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation failed for one or more fields.",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}

/// <summary>
/// 4. Null Payload Guard Filter (ActionFilterAttribute)
/// Guards against empty or non-matching request bodies when clients post empty JSON.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ValidateNullPayloadAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is null)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ValidateNullPayloadAttribute>>();
                logger?.LogWarning("[ValidateNullPayload] Argument '{Key}' is null on {Path}", argument.Key, context.HttpContext.Request.Path);

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Missing Request Body",
                    Detail = $"The request body for parameter '{argument.Key}' cannot be null or empty.",
                    Instance = context.HttpContext.Request.Path,
                    Type = "https://httpstatuses.com/400"
                };
                problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                context.Result = new BadRequestObjectResult(problem);
                return;
            }
        }
    }
}

/// <summary>
/// 5. Route Parameter Validation Filter (ActionFilterAttribute)
/// Ensures route IDs are positive integers before querying databases or services.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ValidatePositiveIdAttribute : ActionFilterAttribute
{
    private readonly string _parameterName;

    public ValidatePositiveIdAttribute(string parameterName = "id")
    {
        _parameterName = parameterName;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue(_parameterName, out var value) && value is int id && id <= 0)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<ValidatePositiveIdAttribute>>();
            logger?.LogWarning("[ValidatePositiveId] Parameter '{ParamName}' has invalid value '{Value}'", _parameterName, id);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid Identifier",
                Detail = $"The route parameter '{_parameterName}' must be a positive integer greater than 0.",
                Instance = context.HttpContext.Request.Path,
                Type = "https://httpstatuses.com/400"
            };
            problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new BadRequestObjectResult(problem);
        }
    }
}
