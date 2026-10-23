#nullable enable
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCore.Filters;

/// <summary>
/// 1. Synchronous Role/Authentication Authorization Filter (IAuthorizationFilter)
/// Runs first in the filter pipeline to verify user authentication/claims.
/// Emits structured RFC-7807 ProblemDetails on authorization failure.
/// </summary>
public class CustomAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _requiredRole;
    private readonly ILogger<CustomAuthorizationFilter> _logger;

    public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger, string requiredRole = "Admin")
    {
        _logger = logger;
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var path = context.HttpContext.Request.Path;
        var traceId = context.HttpContext.TraceIdentifier;

        // 1. Verify Authentication
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("[Authorization] Unauthenticated request rejected for {Path} (TraceId: {TraceId})", path, traceId);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Authentication credentials are required to access this endpoint.",
                Instance = path,
                Type = "https://httpstatuses.com/401"
            };
            problemDetails.Extensions["traceId"] = traceId;

            context.Result = new UnauthorizedObjectResult(problemDetails);
            return;
        }

        // 2. Verify Role Authorization
        if (!string.IsNullOrEmpty(_requiredRole) && !user.IsInRole(_requiredRole))
        {
            _logger.LogWarning("[Authorization] User '{UserName}' lacks required role '{Role}' for {Path} (TraceId: {TraceId})",
                user.Identity.Name ?? "Unknown", _requiredRole, path, traceId);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Detail = $"Access denied. You must belong to the '{_requiredRole}' role to perform this action.",
                Instance = path,
                Type = "https://httpstatuses.com/403"
            };
            problemDetails.Extensions["traceId"] = traceId;

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        _logger.LogDebug("[Authorization] Request authorized for user '{UserName}' on {Path}", user.Identity?.Name, path);
    }
}

/// <summary>
/// 2. Asynchronous API Key Authorization Filter (IAsyncAuthorizationFilter)
/// Production-grade API Key validator using constant-time comparison (CryptographicOperations.FixedTimeEquals)
/// to prevent timing-based side-channel attacks.
/// </summary>
public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyAuthorizationFilter> _logger;

    public ApiKeyAuthorizationFilter(IConfiguration configuration, ILogger<ApiKeyAuthorizationFilter> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var path = context.HttpContext.Request.Path;
        var traceId = context.HttpContext.TraceIdentifier;

        // 1. Check if header exists
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) ||
            string.IsNullOrWhiteSpace(extractedApiKey))
        {
            _logger.LogWarning("[ApiKeyAuth] Missing '{Header}' header for {Path} (TraceId: {TraceId})", ApiKeyHeaderName, path, traceId);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Missing API Key",
                Detail = $"API Key is required. Please supply a valid '{ApiKeyHeaderName}' header.",
                Instance = path,
                Type = "https://httpstatuses.com/401"
            };
            problem.Extensions["traceId"] = traceId;

            context.Result = new UnauthorizedObjectResult(problem);
            return Task.CompletedTask;
        }

        // 2. Retrieve expected key from configuration
        var expectedApiKey = _configuration.GetValue<string>("Authentication:ApiKey") ?? "SecretKey12345";

        // 3. Constant-time comparison to prevent timing attacks
        var expectedBytes = Encoding.UTF8.GetBytes(expectedApiKey);
        var extractedBytes = Encoding.UTF8.GetBytes(extractedApiKey.ToString());

        if (expectedBytes.Length != extractedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(expectedBytes, extractedBytes))
        {
            _logger.LogWarning("[ApiKeyAuth] Invalid API key supplied for {Path} (TraceId: {TraceId})", path, traceId);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Invalid API Key",
                Detail = "The supplied API key is invalid or has expired.",
                Instance = path,
                Type = "https://httpstatuses.com/403"
            };
            problem.Extensions["traceId"] = traceId;

            context.Result = new ObjectResult(problem)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return Task.CompletedTask;
        }

        _logger.LogDebug("[ApiKeyAuth] API Key verified for {Path}", path);
        return Task.CompletedTask;
    }
}

/// <summary>
/// 3. Convenient Attribute for API Key Authorization
/// Resolves ApiKeyAuthorizationFilter and its dependencies via DI.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : TypeFilterAttribute
{
    public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
