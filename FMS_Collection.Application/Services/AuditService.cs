using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FMS_Collection.Application.Services
{
    public class AuditService(IAuditLogRepository auditLogRepository, ILogger<AuditService> logger) : IAuditService
    {
        public async Task LogAsync(
            Guid? userId,
            string action,
            string? entityType = null,
            string? entityId = null,
            object? oldValues = null,
            object? newValues = null,
            string? ipAddress = null,
            string? userAgent = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                    NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    CreatedOn = DateTime.UtcNow
                };

                await auditLogRepository.AddAsync(auditLog);
            }
            catch (Exception ex)
            {
                // Audit logging should never throw — log and continue
                logger.LogWarning(ex, "Failed to write audit log for action {Action}", action);
            }
        }
    }
}
