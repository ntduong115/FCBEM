using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FCCore.Middlewares
{
    public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred.");
                throw; // Re-throw the exception after logging it
            }
        }
    }
}
