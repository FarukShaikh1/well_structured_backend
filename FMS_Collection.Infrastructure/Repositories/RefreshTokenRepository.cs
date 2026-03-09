using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class RefreshTokenRepository(DbConnectionFactory dbFactory) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RefreshToken_GetByToken", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_Token", SqlDbType.NVarChar, 512) { Value = token });
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new RefreshToken
                {
                    Id = (Guid)reader["Id"],
                    UserId = (Guid)reader["UserId"],
                    Token = reader["Token"].ToString()!,
                    ExpiresOn = (DateTime)reader["ExpiresOn"],
                    CreatedOn = (DateTime)reader["CreatedOn"],
                    RevokedOn = reader["RevokedOn"] != DBNull.Value ? (DateTime?)reader["RevokedOn"] : null
                };
            }
            return null;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RefreshToken_Add", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = refreshToken.UserId });
            cmd.Parameters.Add(new SqlParameter("@in_Token", SqlDbType.NVarChar, 512) { Value = refreshToken.Token });
            cmd.Parameters.Add(new SqlParameter("@in_ExpiresOn", SqlDbType.DateTime2) { Value = refreshToken.ExpiresOn });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RevokeAsync(string token)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RefreshToken_Revoke", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_Token", SqlDbType.NVarChar, 512) { Value = token });
            cmd.Parameters.Add(new SqlParameter("@in_Reason", SqlDbType.NVarChar, 512) { Value = "Some Error" });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RevokeAllForUserAsync(Guid userId)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RefreshToken_RevokeAllForUser", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
            cmd.Parameters.Add(new SqlParameter("@in_RevokedOn", SqlDbType.DateTime2) { Value = DateTime.UtcNow });
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteExpiredAsync()
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd = new SqlCommand("RefreshToken_DeleteExpired", conn) { CommandType = CommandType.StoredProcedure };
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
