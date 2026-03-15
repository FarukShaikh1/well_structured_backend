using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class AuditLogRepository(DbConnectionFactory dbFactory) : IAuditLogRepository
    {
        public async Task AddAsync(AuditLog auditLog)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("AuditLog_Add", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = (object?)auditLog.UserId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_Action", SqlDbType.NVarChar, 100) { Value = auditLog.Action });
            cmd.Parameters.Add(new SqlParameter("@in_EntityType", SqlDbType.NVarChar, 100) { Value = (object?)auditLog.EntityType ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_EntityId", SqlDbType.NVarChar, 100) { Value = (object?)auditLog.EntityId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_OldValues", SqlDbType.NVarChar, -1) { Value = (object?)auditLog.OldValues ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_NewValues", SqlDbType.NVarChar, -1) { Value = (object?)auditLog.NewValues ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_IpAddress", SqlDbType.NVarChar, 50) { Value = (object?)auditLog.IpAddress ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_UserAgent", SqlDbType.NVarChar, 500) { Value = (object?)auditLog.UserAgent ?? DBNull.Value });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<AuditLogResponse>> GetAsync(int pageNumber, int pageSize, Guid? userId = null, string? action = null)
        {
            var list = new List<AuditLogResponse>();
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("AuditLog_Get", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_PageNum", SqlDbType.Int) { Value = pageNumber });
            cmd.Parameters.Add(new SqlParameter("@in_PageSize", SqlDbType.Int) { Value = pageSize });
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = (object?)userId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_Action", SqlDbType.NVarChar, 100) { Value = (object?)action ?? DBNull.Value });
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new AuditLogResponse
                {
                    Id = (long)reader["Id"],
                    UserId = reader["UserId"] != DBNull.Value ? (Guid?)reader["UserId"] : null,
                    UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null,
                    Action = reader["Action"].ToString()!,
                    EntityType = reader["EntityType"] != DBNull.Value ? reader["EntityType"].ToString() : null,
                    EntityId = reader["EntityId"] != DBNull.Value ? reader["EntityId"].ToString() : null,
                    IpAddress = reader["IpAddress"] != DBNull.Value ? reader["IpAddress"].ToString() : null,
                    CreatedOn = (DateTime)reader["CreatedOn"]
                });
            }
            return list;
        }

        public async Task<int> GetCountAsync(Guid? userId = null, string? action = null)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("AuditLog_GetCount", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = (object?)userId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@in_Action", SqlDbType.NVarChar, 100) { Value = (object?)action ?? DBNull.Value });
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }
}
