using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog auditLog);
        Task<List<AuditLogResponse>> GetAsync(int pageNumber, int pageSize, Guid? userId = null, string? action = null);
        Task<int> GetCountAsync(Guid? userId = null, string? action = null);
    }
}
