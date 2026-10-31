using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApiCore.Middleware
{
    public static class RequestLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Adds the RequestLoggingMiddleware to the ASP.NET Core pipeline.
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}

