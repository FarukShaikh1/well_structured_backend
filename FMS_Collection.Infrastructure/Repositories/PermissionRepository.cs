using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class PermissionRepository(DbConnectionFactory dbFactory) : IPermissionRepository
    {
        public async Task<List<PermissionResponse>> GetAllAsync()
        {
            var list = new List<PermissionResponse>();
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_GetAll", conn) { CommandType = CommandType.StoredProcedure };
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapPermission(reader));
            return list;
        }

        public async Task<List<PermissionResponse>> GetByRoleAsync(Guid roleId)
        {
            var list = new List<PermissionResponse>();
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_GetByRole", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = roleId });
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapPermission(reader));
            return list;
        }

        public async Task<List<string>> GetPermissionNamesByRoleAsync(Guid roleId)
        {
            var list = new List<string>();
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_GetNamesByRole", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = roleId });
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader["PermissionName"].ToString()!);
            return list;
        }

        public async Task<Guid> AddAsync(PermissionRequest request, Guid createdBy)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_Add", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_PermissionName", SqlDbType.NVarChar, 100) { Value = request.PermissionName });
            cmd.Parameters.Add(new SqlParameter("@in_Module", SqlDbType.NVarChar, 100) { Value = request.Module });
            cmd.Parameters.Add(new SqlParameter("@in_Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_IsActive", SqlDbType.Bit) { Value = request.IsActive });
            cmd.Parameters.Add(new SqlParameter("@in_CreatedBy", SqlDbType.UniqueIdentifier) { Value = createdBy });
            var outId = new SqlParameter("@out_PermissionId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (Guid)(outId.Value ?? Guid.Empty);
        }

        public async Task UpdateAsync(Guid permissionId, PermissionRequest request, Guid updatedBy)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_Update", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_PermissionId", SqlDbType.UniqueIdentifier) { Value = permissionId });
            cmd.Parameters.Add(new SqlParameter("@in_PermissionName", SqlDbType.NVarChar, 100) { Value = request.PermissionName });
            cmd.Parameters.Add(new SqlParameter("@in_Module", SqlDbType.NVarChar, 100) { Value = request.Module });
            cmd.Parameters.Add(new SqlParameter("@in_Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_IsActive", SqlDbType.Bit) { Value = request.IsActive });
            cmd.Parameters.Add(new SqlParameter("@in_ModifiedBy", SqlDbType.UniqueIdentifier) { Value = updatedBy });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(Guid permissionId)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Permission_Delete", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_PermissionId", SqlDbType.UniqueIdentifier) { Value = permissionId });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AssignPermissionsToRoleAsync(AssignRolePermissionsRequest request, Guid assignedBy)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RolePermission_Assign", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = request.RoleId });
            cmd.Parameters.Add(new SqlParameter("@in_AssignedBy", SqlDbType.UniqueIdentifier) { Value = assignedBy });

            // Pass permission IDs as comma-separated string (SP will split them)
            var ids = string.Join(",", request.PermissionIds.Select(x => x.ToString()));
            cmd.Parameters.Add(new SqlParameter("@in_PermissionIds", SqlDbType.NVarChar, -1) { Value = ids });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RevokePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RolePermission_Revoke", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = roleId });
            cmd.Parameters.Add(new SqlParameter("@in_PermissionId", SqlDbType.UniqueIdentifier) { Value = permissionId });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static PermissionResponse MapPermission(SqlDataReader reader) => new()
        {
            PermissionId = (Guid)reader["PermissionId"],
            PermissionName = reader["PermissionName"].ToString()!,
            Module = reader["Module"].ToString()!,
            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
            IsActive = (bool)reader["IsActive"],
            CreatedOn = (DateTime)reader["CreatedOn"]
        };
    }
}
