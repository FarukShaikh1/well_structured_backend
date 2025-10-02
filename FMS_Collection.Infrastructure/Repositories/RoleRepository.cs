using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public RoleRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = new List<Role>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    roles.Add(new Role
                    {
                        RoleId = reader.GetGuid(reader.GetOrdinal("Id")),
                        RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                        RoleDescription = reader.IsDBNull(reader.GetOrdinal("RoleDescription")) ? null : reader.GetString(reader.GetOrdinal("RoleDescription")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return roles;
        }

        public async Task<List<RoleListResponse>> GetRoleListAsync()
        {
            var roles = new List<RoleListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    roles.Add(new RoleListResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        RoleName = reader["RoleName"]?.ToString(),
                        RoleDescription = reader["RoleDescription"]?.ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return roles;
        }

        public async Task<RoleDetailsResponse> GetRoleDetailsAsync(Guid roleId)
        {
            var result = new RoleDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = roleId });

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new RoleDetailsResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName")),
                        RoleDescription = reader.IsDBNull(reader.GetOrdinal("RoleDescription")) ? null : reader.GetString(reader.GetOrdinal("RoleDescription"))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return result;
        }

        public async Task AddAsync(RoleRequest role, Guid roleId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                AddRoleRequestParameters(cmd, role, roleId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task UpdateAsync(RoleRequest role, Guid roleId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddRoleRequestParameters(cmd, role, roleId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid roleId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Role_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@in_RoleId", SqlDbType.UniqueIdentifier) { Value = roleId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                await conn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();

                return result != null && Convert.ToInt32(result) == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        private void AddRoleRequestParameters(SqlCommand cmd, RoleRequest role, Guid userId)
        {
            cmd.Parameters.Add(new SqlParameter("@in_RoleName", SqlDbType.VarChar) { Value = role.RoleName });
            cmd.Parameters.Add(new SqlParameter("@in_RoleDescription", SqlDbType.VarChar) { Value = role.RoleDescription });
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
        }
    }
}
