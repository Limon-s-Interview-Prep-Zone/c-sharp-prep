#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Authorization Filter (IAuthorizationFilter)
/// Runs first in the filter pipeline to verify user authentication/claims.
/// Can short-circuit immediately with 401 Unauthorized or 403 Forbidden.
/// </summary>
public class CustomAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _requiredRole;

    public CustomAuthorizationFilter(string requiredRole = "Admin")
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        Console.WriteLine("[CustomAuthorizationFilter] Checking user authentication and roles...");

        var user = context.HttpContext.User;

        // 1. Check if user is authenticated
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            Console.WriteLine("[CustomAuthorizationFilter] User is not authenticated. Short-circuiting with 401 Unauthorized.");
            context.Result = new UnauthorizedObjectResult(new
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Authentication is required to access this resource."
            });
            return;
        }

        // 2. Check role authorization if specified
        if (!string.IsNullOrEmpty(_requiredRole) && !user.IsInRole(_requiredRole))
        {
            Console.WriteLine($"[CustomAuthorizationFilter] User lacks required role '{_requiredRole}'. Short-circuiting with 403 Forbidden.");
            context.Result = new ForbidResult();
            return;
        }

        Console.WriteLine("[CustomAuthorizationFilter] Authorization successful. Proceeding down the pipeline.");
    }
}

/// <summary>
/// 2. Asynchronous API Key Authorization Filter (IAsyncAuthorizationFilter)
/// Validates an 'X-Api-Key' header on incoming requests.
/// Frequently used in B2B integrations, microservices, and webhook endpoints.
/// </summary>
public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthorizationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        Console.WriteLine("[ApiKeyAuthorizationFilter] Inspecting request headers for API key...");

        // 1. Check if the header is present
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            Console.WriteLine("[ApiKeyAuthorizationFilter] Missing 'X-Api-Key' header. Short-circuiting with 401.");
            context.Result = new UnauthorizedObjectResult(new
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = $"API Key is missing. Please provide a valid '{ApiKeyHeaderName}' header."
            });
            return;
        }

        // 2. Retrieve expected key from configuration or secret store
        var expectedApiKey = _configuration.GetValue<string>("Authentication:ApiKey") ?? "SecretKey12345";

        // Simulate async lookup (e.g. database/cache validation)
        await Task.CompletedTask;

        // 3. Constant-time or direct string comparison
        if (!string.Equals(expectedApiKey, extractedApiKey))
        {
            Console.WriteLine("[ApiKeyAuthorizationFilter] Invalid API Key. Short-circuiting with 403 Forbidden.");
            context.Result = new ObjectResult(new
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "Invalid API Key provided. Access denied."
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        Console.WriteLine("[ApiKeyAuthorizationFilter] API Key verified successfully.");
    }
}

/// <summary>
/// 3. Convenient Attribute for API Key Authorization
/// Allows easy decoration on controllers or actions: [ApiKeyAuth]
/// Uses TypeFilterAttribute to resolve IConfiguration via Dependency Injection.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : TypeFilterAttribute
{
    public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
