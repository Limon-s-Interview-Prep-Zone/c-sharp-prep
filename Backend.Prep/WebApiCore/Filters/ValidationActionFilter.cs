using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Validation Filter (IActionFilter)
/// Checks ModelState.IsValid and short-circuits the pipeline before the action runs.
/// </summary>
public class ValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"[ValidationActionFilter] OnActionExecuting called. ModelState.IsValid: {context.ModelState.IsValid}");
        if (!context.ModelState.IsValid)
        {
            Console.WriteLine("[ValidationActionFilter] Validation error detected. Returning BadRequest.");
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("[ValidationActionFilter] OnActionExecuted called.");
    }
}

/// <summary>
/// 2. Asynchronous Validation Filter (IAsyncActionFilter)
/// Idiomatic modern .NET async pattern for non-blocking execution.
/// </summary>
public class AsyncValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine($"[AsyncValidationActionFilter] Checking ModelState.IsValid: {context.ModelState.IsValid}");

        if (!context.ModelState.IsValid)
        {
            Console.WriteLine("[AsyncValidationActionFilter] Validation failed. Short-circuiting pipeline.");
            context.Result = new BadRequestObjectResult(context.ModelState);
            return; // Short-circuit: do NOT call next()
        }

        // Proceed to next filter or action method
        var executedContext = await next();

        Console.WriteLine("[AsyncValidationActionFilter] Action executed successfully.");
    }
}

/// <summary>
/// 3. Attribute-Based Validation Filter (ActionFilterAttribute)
/// Usable directly as [ValidateModel] on specific controllers or action methods.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"[ValidateModelAttribute] Checking ModelState (IsValid: {context.ModelState.IsValid})");
        if (!context.ModelState.IsValid)
        {
            Console.WriteLine("[ValidateModelAttribute] Action-level validation failed.");
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}

/// <summary>
/// 4. Null Payload Validation Filter (ActionFilterAttribute)
/// Guards against empty/null request body when clients post empty JSON or non-matching schemas.
/// Prevents NullReferenceException inside controller action methods.
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
                Console.WriteLine($"[ValidateNullPayloadAttribute] Argument '{argument.Key}' is null. Short-circuiting with 400.");
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = $"Request payload for parameter '{argument.Key}' cannot be null or empty."
                });
                return;
            }
        }
    }
}

/// <summary>
/// 5. Standardized RFC 7807 ProblemDetails Validation Filter
/// Formats validation errors into a consistent, enterprise-grade RFC 7807 ProblemDetails response.
/// </summary>
public class StandardizedValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for detailed validation failures.",
                Instance = context.HttpContext.Request.Path
            };

            Console.WriteLine("[StandardizedValidationFilter] Returning RFC 7807 ValidationProblemDetails.");
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

/// <summary>
/// 6. Route Parameter / ID Validation Filter (ActionFilterAttribute)
/// Validates route parameters (e.g., id must be > 0) before hitting database / services.
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
        if (context.ActionArguments.TryGetValue(_parameterName, out var value) && value is int id)
        {
            if (id <= 0)
            {
                Console.WriteLine($"[ValidatePositiveIdAttribute] Invalid {_parameterName}: {id}. Must be > 0.");
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = $"The parameter '{_parameterName}' must be a positive integer greater than 0."
                });
            }
        }
    }
}