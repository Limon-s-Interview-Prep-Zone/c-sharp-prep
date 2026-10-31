using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApiCore.Middleware
{
    /// <summary>
    /// Simple request‑logging middleware. Logs the incoming request path and the elapsed time.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

            // Call the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation("Request completed in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}

