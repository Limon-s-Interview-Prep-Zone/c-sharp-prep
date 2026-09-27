# Dotnet Interview Prep

## Attribute:
In ASP.NET Core, attributes are special classes that provide metadata about the code they decorate.
1. `Routing Attribute:` Used to configure `routing` for controllers and actions.
   - `[Route("api/[controller]")]`: Defines the route template.
   - `[HttpGet("{id}")], [HttpPost], [HttpPut], [HttpDelete]`: Specify HTTP methods for the action.
2. `Validation Attribute:` Used for model validation in `MVC or API controllers`.
   - `[Required]`: Ensures a value is provided.
   - `[Range(18,99)]`: Enforces a numeric range.
   - `[StringLength]`: Sets string length constraints.
3. `Filter Attributes`: Used to add custom logic at various stages of the request pipeline.
   - `[Authorize(Roles="")]`: Enforces authorization.
   - `[AllowAnonymous]`: Allows anonymous access to endpoints.
   - `[CustomActionFilter]`: Apply custom action filters.
4. `Binding Attributes`: Used to control model binding in controllers.
   - `[FromQuery]` - Gets values from the `query string`.
   - `[FromRoute]` - Gets values from `route data`.
   - `[FromForm]` - Gets values from posted `form fields`.
   - `[FromBody]` - Gets values from the `request body`.
   - `[FromHeader]` - Gets values from `HTTP headers`.
   - [FromServices] - allow injecting dependencies directly into controller actions.
        ```c#
        [HttpGet]
        public IActionResult GetServiceData([FromServices] IMyService service)
        {
            return Ok(service.GetData());
        }
        ```
5. `Exception Handling Attributes:`
   - `[ApiController]`: Enhances API behavior, such as automatic validation response and binding improvements.
---
## Filter:
Filters are used to execute custom logic before or after an action method in ASP.NET Core applications. They are useful for cross-cutting concerns like `logging, authentication, caching, or input validation`.

**Types of Filters:**
1. **Authorization Filters:** Run before the action to ensure the user is authorized.
2. **Resource Filters:** Run before the `model binding`, often used to `cache or set up` resources.
   - Cache
3. **Action Filters:** Run before and after the action method execution.
4. **Exception Filters:** Handle exceptions thrown by action methods or other filters.
5. **Result Filters:** Run after an action returns a result but before the result is sent to the client.
   - APIs where metadata (e.g., pagination info) is added to the response.
   - Adding custom `HTTP headers` to all responses.
   - Standardizing `all responses` to follow a specific format
   - Logging the final response before it is sent to the client.

**Filter Example**
```c#
public class CustomActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Logic before the action method executes
        Console.WriteLine("Action is executing...");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Logic after the action method executes
        Console.WriteLine("Action executed.");
    }
}

public class MyAsyncActionFilter : IAsyncActionFilter
{
    // Runs asynchronously before and after the action method execution
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("Before executing async action.");

        // Add any async pre-action logic here
        // For example, async I/O, database calls, etc.
        await Task.Delay(100); // Simulate async work

        // Calling the next delegate in the pipeline
        var executedContext = await next();

        _logger.LogInformation("After executing async action.");
        // Add any async post-action logic here
    }
}

//DI 
services.AddControllers(options =>
{
    options.Filters.Add<CustomActionFilter>();//globally
});
//register
builder.Services.AddScoped<MyAsyncActionFilter>();

```
1. **ServiceFilter** allows you to use filters that depend on services registered in the DI container.`[ServiceFilter(typeof(AsyncLogActionFilter))] // Using DI filter`
2. **Typefilter:** Use [TypeFilter] for additional parameters or complex scenarios.`[TypeFilter(typeof(ParameterizedLogFilter), Arguments = new object[] { "Order Processing" })]`
3. **ActionFilterAttribute**: Filters can be implemented as `attributes` by inheriting from `ActionFilterAttribute` (or other filter base classes like Attribute combined with filter interfaces).
   
---
## Middleware:
Middleware is a pipeline component that `processes requests and responses`. Each request passes through a series of middleware before reaching the final handler (controller or endpoint).
   1. Middleware can inspect, modify, or short-circuit requests.
   2. Each middleware calls the next middleware or stops the pipeline.

**Example:**
```c#
public class CustomHeaderMiddlewareAsync
{
    private readonly RequestDelegate _next;

    public CustomHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context); // Process request and move to the next middleware

        // Modify the response by adding custom headers
        context.Response.Headers["X-Custom-Header"] = "Header Value";
        context.Response.Headers["X-Request-ID"] = Guid.NewGuid().ToString();
    }
}
//sync middleware
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        await _next(context); // Call the next middleware
        Console.WriteLine($"Response: {context.Response.StatusCode}");
    }
}

// Register middleware in Program.cs or Startup.cs
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<CustomHeaderMiddleware>();

```

---
## Background Service
A Background Service is a long-running task that operates independently from the main request-response pipeline. ASP.NET Core provides BackgroundService as a base class to simplify the creation of such services. It is normally used in 

1. Data Synchronization: Sync data with a third-party API periodically.
2. Task Scheduling: Send email notifications at specific intervals.
3. Monitoring Services: Continuously monitor system health.

#### **Example:** `Background Service`
BackgroundService is an abstract base class that provides a simpler way to implement background tasks by encapsulating the IHostedService logic.
```c#
public class DataSyncService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Syncing data...");
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Wait 1 hour
        }
    }
}
// Register the service
services.AddHostedService<DataSyncService>();
```

#### **Example:** `IHostedService`
Used to start and stop long-running background tasks. This interface provides more control but requires more effort to implement compared to BackgroundService.
```c#
public class TimedHostedService : IHostedService, IDisposable
{
    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Timed Background Service is starting.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Console.WriteLine($"Background task executed at: {DateTime.Now}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Timed Background Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
```

---
## Worker Service:
A Worker Service is a project template in .NET designed for running long-running background processes. It provides the necessary configuration to run your service as:
- A standalone console application.
- A Windows Service or Linux daemon.
- It is ideal for background applications that need to `run independently of web servers or APIs`, such as `monitoring, logging, or batch processing`.

**Example:** in console application
```c#
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>(); // Register the worker service
            });
}
```

---
## DI
Dependency Injection (DI) is a design pattern used in software development to achieve `Inversion of Control (IoC)`. It allows a class to `receive its dependencies from an external source` rather `than creating them internally`. This approach promotes loose coupling between components, enhances `code modularity`, and `improves testability`.
### Singleton: One instance for the entire application lifecycle
A singleton service is created once and reused for the entire lifetime of the application (until the app shuts down). All classes that request this service will receive the same instance.
1. Use Singleton for stateless services (e.g., logging, caching) that don’t need to change during the application’s lifetime.

### Scoped: One instance per HTTP request:
A scoped service is created once per HTTP request. During a request, all classes that depend on the scoped service will share the same instance. However, a new instance will be created for the next request.

**Behaviors:**
1. If multiple services or controllers request `IOrderService` within the same HTTP request, they will receive the same instance.
2. For the next HTTP request, a new instance will be created.

##### How It Works at Runtime
1. `Request 1: First HTTP Request`
   1. When the user adds an item to the `cart (CartController.AddToCart())`:
      1. A new instance of `OrderService` is created for this request `(say, Instance A)`.
      2. ServiceId (e.g., 2a3d) is assigned to this instance.
   2. When the user proceeds to `checkout (CheckoutController.PlaceOrder())`:
      1. The `same OrderService` instance (Instance A) will be used for this HTTP request.
2. `Request 2: Second HTTP Request`
   1. In a new HTTP request, when the user adds another `item or checks out`:
      1. A new instance of `OrderService (say, Instance B)` is created for this new request.
      2. ServiceId (e.g., 5f8e) is assigned to this new instance.

### Transient: A new instance every time the service is requested
A transient service is created every time it is requested from the DI container. This means that every dependent class or service will receive a new instance.

**Behaviors:**
1. Each time `INotificationService` is requested (even within the same HTTP request), a new instance will be created.
2. If two controllers or two services request the same transient service, they will each get a `separate instance`.

**When do Use Transient?**
1. Use transient services when stateless behavior is needed, and a new instance is required every time, such as lightweight services with no shared state.


---
## Transaction
A transaction is a sequence of operations that are treated as a `single unit`. The operations inside a transaction must meet the `ACID` properties:

1. `Atomicity:` All operations must succeed, or none of them should apply.
2. `Consistency:` The database moves from one consistent state to another.
3. `Isolation:` Concurrent transactions are isolated from each other.
4. `Durability:` Changes are permanent after the transaction is committed.

If one operation fails, all changes are rolled back.

### Approach 1: Using EF Core Transactions:
```c#
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // Add order to the database
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();

    // Add payment to the database
    _context.Payments.Add(payment);
    await _context.SaveChangesAsync();

    // Commit the transaction
    await transaction.CommitAsync();
    return true;
}
catch (Exception)
{
    // Roll back the transaction on error
    await transaction.RollbackAsync();
    return false;
}
```

### Approach 2: TransactionScope in .NET Core:

You can also use TransactionScope from System.Transactions. This approach allows you to wrap operations across multiple DbContexts or external services in a single transaction.
```c#
using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
try
{
    _context1.Orders.Add(order);
    await _context1.SaveChangesAsync();

    _context2.Payments.Add(payment);
    await _context2.SaveChangesAsync();

    scope.Complete();  // Commit the transaction
    return true;
}
catch (Exception)
{
    // No explicit rollback needed; scope will auto-revert if not completed
    return false;
}
```

### Approach 3: Unit of Work with Repository Pattern
When following Domain-Driven Design (DDD) or Clean Architecture, you can implement the Unit of Work (UoW) pattern to group multiple repositories under a single transactional scope.
```c#
public async Task<int> CompleteAsync()
{
    return await _context.SaveChangesAsync();
}
```

### Managing transaction in Microservice:
1. Two-Phase Commit (2PC)
2. SAGA Pattern
3. Outbox Pattern
4. Distributed Transactions with gRPC/RabbitMQ