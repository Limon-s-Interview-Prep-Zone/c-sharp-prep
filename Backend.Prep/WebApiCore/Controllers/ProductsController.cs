using Microsoft.AspNetCore.Mvc;
using WebApiCore.Entities;
using WebApiCore.Filters;

namespace WebApiCore.Controllers;

/// <summary>
/// ProductsController demonstrating the practical usage of all 5 ASP.NET Core filter types:
/// 1. Authorization Filter: [ApiKeyAuth], [TypeFilter(typeof(CustomAuthorizationFilter))]
/// 2. Resource Filter:      [AsyncCacheResourceFilter]
/// 3. Action Filter:        [TypeFilter(typeof(ExecutionTimerActionFilter))], [AuditLog], [ValidatePositiveId], [ValidateNullPayload]
/// 4. Exception Filter:     [ApiExceptionHandling]
/// 5. Result Filter:        [ResponseHeader], [TypeFilter(typeof(StandardizedResponseEnvelopeFilter))]
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> ProductsDatabase = new()
    {
        new Product { Id = 1, Name = "iPhone 15 Pro", Price = 999 },
        new Product { Id = 2, Name = "MacBook Pro M3", Price = 1999 },
        new Product { Id = 3, Name = "iPad Pro", Price = 799 }
    };

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 1. Usage of RESOURCE FILTER & RESULT FILTER:
    /// - [AsyncCacheResourceFilter]: Runs BEFORE model binding, caches output in memory for 30 seconds.
    /// - [ResponseHeader]: Injects a custom HTTP response header in OnResultExecuting.
    /// </summary>
    [HttpGet]
    [AsyncCacheResourceFilter(durationSeconds: 30)]
    [ResponseHeader("X-Resource-Strategy", "MemoryCachePreModelBinding")]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        _logger.LogInformation("[ProductsController] Executing GetAll - (Will be skipped on cache HIT)");
        return Ok(ProductsDatabase);
    }

    /// <summary>
    /// 2. Usage of ACTION FILTERS & RESULT ENVELOPE FILTER:
    /// - [ValidatePositiveId]: Action filter validating 'id > 0' before method runs.
    /// - [TypeFilter(typeof(ExecutionTimerActionFilter))]: Measures execution latency and adds 'X-Action-Time-Ms'.
    /// - [TypeFilter(typeof(StandardizedResponseEnvelopeFilter))]: Wraps response into { success, data, traceId, timestamp }.
    /// </summary>
    [HttpGet("{id:int}")]
    [ValidatePositiveId("id")]
    [TypeFilter(typeof(ExecutionTimerActionFilter))]
    [TypeFilter(typeof(StandardizedResponseEnvelopeFilter))]
    public ActionResult<Product> GetById(int id)
    {
        _logger.LogInformation("[ProductsController] Querying product with Id: {Id}", id);

        var product = ProductsDatabase.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            // Will be wrapped in envelope as not found or return 404
            return NotFound(new { Message = $"Product with Id '{id}' not found." });
        }

        return Ok(product);
    }

    /// <summary>
    /// 3. Usage of ACTION VALIDATION & AUDIT LOGGING FILTERS:
    /// - [AuditLog]: Action filter logging caller identity and arguments.
    /// - [ValidateNullPayload]: Guards against empty/null request body.
    /// - [ValidateModel]: Validates ModelState.IsValid constraints ([Required], etc.).
    /// </summary>
    [HttpPost]
    [AuditLog]
    [ValidateNullPayload]
    [ValidateModel]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        _logger.LogInformation("[ProductsController] Adding new product: {Name}", product.Name);

        product.Id = ProductsDatabase.Max(p => p.Id) + 1;
        ProductsDatabase.Add(product);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// 4. Usage of AUTHORIZATION FILTER:
    /// - [ApiKeyAuth]: Validates incoming 'X-Api-Key' request header.
    ///   Short-circuits immediately with 401 Unauthorized if missing, or 403 Forbidden if invalid.
    /// </summary>
    [HttpGet("secure/catalog")]
    [ApiKeyAuth]
    [ResponseHeader("X-Access-Tier", "ApiKeySecured")]
    public ActionResult<IEnumerable<Product>> GetSecuredCatalog()
    {
        _logger.LogInformation("[ProductsController] Accessing confidential product catalog via valid API Key.");
        return Ok(new
        {
            Message = "Access Granted via ApiKeyAuthorizationFilter",
            ConfidentialCatalog = ProductsDatabase
        });
    }

    /// <summary>
    /// 5. Usage of ROLE AUTHORIZATION with Parameters via TypeFilter:
    /// - [TypeFilter(typeof(CustomAuthorizationFilter), Arguments = ...)]: Requires authenticated user in 'Admin' role.
    /// </summary>
    [HttpDelete("{id:int}")]
    [TypeFilter(typeof(CustomAuthorizationFilter), Arguments = new object[] { "Admin" })]
    [ValidatePositiveId("id")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("[ProductsController] Deleting product {Id} by Admin.", id);

        var product = ProductsDatabase.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        ProductsDatabase.Remove(product);
        return NoContent();
    }

    /// <summary>
    /// 6. Usage of EXCEPTION FILTER:
    /// - [ApiExceptionHandling]: Catches unhandled exceptions and translates them into
    ///   RFC-7807 ProblemDetails with appropriate HTTP status codes (KeyNotFoundException -> 404).
    /// </summary>
    [HttpGet("simulate-error/{type}")]
    [ApiExceptionHandling]
    public IActionResult SimulateException(string type)
    {
        _logger.LogInformation("[ProductsController] Simulating exception type: {Type}", type);

        throw type.ToLower() switch
        {
            "notfound" => new KeyNotFoundException($"Product item '{type}' was not located in inventory."),
            "badrequest" => new ArgumentException("The requested product parameter is malformed."),
            "unauthorized" => new UnauthorizedAccessException("You do not have rights to view this product."),
            _ => new InvalidOperationException("An unhandled internal domain error was triggered!")
        };
    }

    /// <summary>
    /// 7. Usage of SERVICE FILTER (DI-Resolved Filter):
    /// - [ServiceFilter(typeof(CacheAsyncActionFilter))]: Resolves the filter instance directly from the DI container.
    /// </summary>
    [HttpGet("service-filter-demo")]
    [ServiceFilter(typeof(CacheAsyncActionFilter))]
    public IActionResult ServiceFilterDemo()
    {
        _logger.LogInformation("[ProductsController] Executing ServiceFilterDemo.");
        return Ok(new { Message = "Successfully resolved and executed CacheAsyncActionFilter via ServiceFilter." });
    }
}
