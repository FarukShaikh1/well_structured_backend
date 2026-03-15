namespace FMS_Collection.Core.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(Guid? userId, string action, string? entityType = null, string? entityId = null,
            object? oldValues = null, object? newValues = null, string? ipAddress = null, string? userAgent = null);
    }
}
