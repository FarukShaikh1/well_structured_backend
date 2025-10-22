using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public OtpRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime ExpiresAt)> Store = new();

        public async Task<Guid> SetAsync(string userKey, string otpCode, string purpose, DateTime expiresOn, Guid? createdBy)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("UserOtpStore_Set", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("@In_UserKey", userKey);
                cmd.Parameters.AddWithValue("@In_OtpCode", otpCode);
                cmd.Parameters.AddWithValue("@In_Purpose", (object?)purpose ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@In_ExpiresOn", expiresOn);
                cmd.Parameters.AddWithValue("@In_CreatedBy", (object?)createdBy ?? DBNull.Value);

                // Output parameter
                var outIdParam = new SqlParameter("@Out_Id", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outIdParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (Guid)(outIdParam.Value ?? Guid.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<(bool Exists, string OtpCode, DateTime ExpiresAt)> GetAsync(string identifier)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("UserOtpStore_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@In_UserKey", identifier);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var otpCode = reader["OtpCode"]?.ToString();
                    var expiresOn = reader["ExpiresOn"] != DBNull.Value
                        ? (DateTime)reader["ExpiresOn"]
                        : DateTime.MinValue;

                    return (true, otpCode, expiresOn);
                }
                return (false, string.Empty, DateTime.MinValue);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task InvalidateAsync(string userKey)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand(@"
            UPDATE [dbo].[UserOtpStore]
            SET IsDeleted = 1,
                ModifiedOn = GETDATE()
            WHERE UserKey = @UserKey AND IsDeleted = 0;", conn);

                cmd.Parameters.AddWithValue("@UserKey", userKey);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(
                    FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

    }
}


