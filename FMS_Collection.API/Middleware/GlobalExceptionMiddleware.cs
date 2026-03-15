using FMS_Collection.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace FMS_Collection.API.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Unhandled exception on {Method} {Path} | User: {UserId} | TraceId: {TraceId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "anonymous",
                    Activity.Current?.Id ?? context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, title) = ex switch
            {
                NotFoundException        => (StatusCodes.Status404NotFound,        "Resource Not Found"),
                UnauthorizedException    => (StatusCodes.Status401Unauthorized,    "Unauthorized"),
                ForbiddenException       => (StatusCodes.Status403Forbidden,       "Access Forbidden"),
                Core.Exceptions.ValidationException => (StatusCodes.Status400BadRequest, "Validation Failed"),
                _                        => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            // Only include error detail in non-production environments
            string? detail = env.IsDevelopment() ? ex.Message : null;

            // Include validation errors if available
            Dictionary<string, string[]>? errors = null;
            if (ex is Core.Exceptions.ValidationException ve && ve.Errors.Any())
                errors = ve.Errors.ToDictionary(k => k.Key, v => v.Value);

            var problem = new
            {
                type = $"https://tools.ietf.org/html/rfc9110#section-{statusCode}",
                title,
                status = statusCode,
                detail,
                errors,
                instance = context.Request.Path.Value,
                traceId = Activity.Current?.Id ?? context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }));
        }
    }
}
