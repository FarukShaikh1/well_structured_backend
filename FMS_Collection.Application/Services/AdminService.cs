// Application/Services/AdminService.cs
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using Microsoft.Extensions.Logging;

namespace FMS_Collection.Application.Services
{
    /// <summary>Business logic for SuperAdmin operations (permissions, role assignments, audit logs).</summary>
    public class AdminService(
        IPermissionRepository permissionRepository,
        IAuditLogRepository auditLogRepository,
        IUserRepository userRepository,
        IAuditService auditService,
        ILogger<AdminService> logger)
    {
        // ── Permissions ───────────────────────────────────────────────────────

        public async Task<ServiceResponse<List<PermissionResponse>>> GetAllPermissionsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => permissionRepository.GetAllAsync(),
                "Permissions fetched successfully.",
                logger);
        }

        public async Task<ServiceResponse<List<PermissionResponse>>> GetPermissionsByRoleAsync(Guid roleId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => permissionRepository.GetByRoleAsync(roleId),
                "Role permissions fetched successfully.",
                logger);
        }

        public async Task<ServiceResponse<Guid>> AddPermissionAsync(PermissionRequest request, Guid createdBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                async () =>
                {
                    var id = await permissionRepository.AddAsync(request, createdBy);
                    await auditService.LogAsync(createdBy, "PermissionCreated", "Permission", id.ToString(),
                        newValues: new { request.PermissionName, request.Module });
                    return id;
                },
                "Permission created successfully.",
                logger);
        }

        public async Task<ServiceResponse<bool>> UpdatePermissionAsync(Guid permissionId, PermissionRequest request, Guid updatedBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                async () =>
                {
                    await permissionRepository.UpdateAsync(permissionId, request, updatedBy);
                    await auditService.LogAsync(updatedBy, "PermissionUpdated", "Permission", permissionId.ToString());
                    return true;
                },
                "Permission updated successfully.",
                logger);
        }

        public async Task<ServiceResponse<bool>> DeletePermissionAsync(Guid permissionId, Guid deletedBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                async () =>
                {
                    await permissionRepository.DeleteAsync(permissionId);
                    await auditService.LogAsync(deletedBy, "PermissionDeleted", "Permission", permissionId.ToString());
                    return true;
                },
                "Permission deleted successfully.",
                logger);
        }

        // ── Role–Permission assignment ─────────────────────────────────────────

        public async Task<ServiceResponse<bool>> AssignPermissionsToRoleAsync(AssignRolePermissionsRequest request, Guid assignedBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                async () =>
                {
                    await permissionRepository.AssignPermissionsToRoleAsync(request, assignedBy);
                    await auditService.LogAsync(assignedBy, "RolePermissionsAssigned", "RolePermission",
                        request.RoleId.ToString(), newValues: new { request.PermissionIds });
                    return true;
                },
                "Permissions assigned to role successfully.",
                logger);
        }

        public async Task<ServiceResponse<bool>> RevokePermissionFromRoleAsync(Guid roleId, Guid permissionId, Guid revokedBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                async () =>
                {
                    await permissionRepository.RevokePermissionFromRoleAsync(roleId, permissionId);
                    await auditService.LogAsync(revokedBy, "RolePermissionRevoked", "RolePermission",
                        $"{roleId}/{permissionId}");
                    return true;
                },
                "Permission revoked from role successfully.",
                logger);
        }

        // ── Audit log ─────────────────────────────────────────────────────────

        public async Task<ServiceResponse<List<AuditLogResponse>>> GetAuditLogsAsync(
            int pageNumber, int pageSize, Guid? userId = null, string? action = null)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => auditLogRepository.GetAsync(pageNumber, pageSize, userId, action),
                "Audit logs fetched successfully.",
                logger);
        }

        public async Task<ServiceResponse<int>> GetAuditLogCountAsync(Guid? userId = null, string? action = null)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => auditLogRepository.GetCountAsync(userId, action),
                "Audit log count fetched successfully.",
                logger);
        }
    }
}
