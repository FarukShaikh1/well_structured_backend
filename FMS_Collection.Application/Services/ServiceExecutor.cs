using FMS_Collection.Core.Common;
using FMS_Collection.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace FMS_Collection.Application.Services
{
    public static class ServiceExecutor
    {
        public static async Task<ServiceResponse<T>> ExecuteAsync<T>(
            Func<Task<T>> action,
            string successMessage,
            ILogger? logger = null)
        {
            try
            {
                var data = await action();
                return ServiceResponse<T>.Ok(data, successMessage);
            }
            catch (NotFoundException ex)
            {
                logger?.LogWarning(ex, "Not found: {Message}", ex.Message);
                return ServiceResponse<T>.Fail(ex.Message, 404);
            }
            catch (ForbiddenException ex)
            {
                logger?.LogWarning(ex, "Forbidden: {Message}", ex.Message);
                return ServiceResponse<T>.Fail(ex.Message, 403);
            }
            catch (Core.Exceptions.ValidationException ex)
            {
                logger?.LogWarning(ex, "Validation error: {Message}", ex.Message);
                return ServiceResponse<T>.Fail(ex.Message, 400);
            }
            catch (UnauthorizedException ex)
            {
                logger?.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
                return ServiceResponse<T>.Fail(ex.Message, 401);
            }
            catch (Exception ex)
            {
                // Log full exception details internally — NEVER expose raw exception message to client
                logger?.LogError(ex, "Unhandled error in ExecuteAsync for {Action}", successMessage);
                return ServiceResponse<T>.Fail("An error occurred while processing your request.", 500);
            }
        }

        public static async Task<ServiceResponse<bool>> ExecuteAsync(
            Func<Task> action,
            string successMessage,
            ILogger? logger = null)
        {
            try
            {
                await action();
                return ServiceResponse<bool>.Ok(true, successMessage);
            }
            catch (NotFoundException ex)
            {
                logger?.LogWarning(ex, "Not found: {Message}", ex.Message);
                return ServiceResponse<bool>.Fail(ex.Message, 404);
            }
            catch (ForbiddenException ex)
            {
                logger?.LogWarning(ex, "Forbidden: {Message}", ex.Message);
                return ServiceResponse<bool>.Fail(ex.Message, 403);
            }
            catch (Core.Exceptions.ValidationException ex)
            {
                logger?.LogWarning(ex, "Validation error: {Message}", ex.Message);
                return ServiceResponse<bool>.Fail(ex.Message, 400);
            }
            catch (UnauthorizedException ex)
            {
                logger?.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
                return ServiceResponse<bool>.Fail(ex.Message, 401);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Unhandled error in ExecuteAsync for {Action}", successMessage);
                return ServiceResponse<bool>.Fail("An error occurred while processing your request.", 500);
            }
        }
    }
}


