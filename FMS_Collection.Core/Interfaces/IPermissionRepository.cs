using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<PermissionResponse>> GetAllAsync();
        Task<List<PermissionResponse>> GetByRoleAsync(Guid roleId);
        Task<List<string>> GetPermissionNamesByRoleAsync(Guid roleId);
        Task<Guid> AddAsync(PermissionRequest request, Guid createdBy);
        Task UpdateAsync(Guid permissionId, PermissionRequest request, Guid updatedBy);
        Task DeleteAsync(Guid permissionId);
        Task AssignPermissionsToRoleAsync(AssignRolePermissionsRequest request, Guid assignedBy);
        Task RevokePermissionFromRoleAsync(Guid roleId, Guid permissionId);
    }
}
