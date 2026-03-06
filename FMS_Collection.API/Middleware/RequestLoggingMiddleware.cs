using System.Diagnostics;

namespace FMS_Collection.API.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            logger.LogInformation(
                "HTTP {Method} {Path} started | TraceId: {TraceId} | IP: {Ip}",
                context.Request.Method,
                context.Request.Path,
                traceId,
                context.Connection.RemoteIpAddress?.ToString() ?? "unknown");

            try
            {
                await next(context);
            }
            finally
            {
                sw.Stop();
                var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                logger.LogInformation(
                    "HTTP {Method} {Path} completed | Status: {StatusCode} | Duration: {Elapsed}ms | User: {UserId} | TraceId: {TraceId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds,
                    userId ?? "anonymous",
                    traceId);
            }
        }
    }
}
