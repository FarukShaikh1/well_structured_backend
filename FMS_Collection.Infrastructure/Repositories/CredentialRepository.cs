using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
//using System.Security.Cryptography;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public CredentialRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // -------------------- GET ALL --------------------
        public async Task<List<CredentialResponse>> GetAllAsync()
        {
            var result = new List<CredentialResponse>();

            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_GetAll", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapCredentialResponse(reader));
            }

            return result;
        }

        // -------------------- GET BY USER --------------------
        public async Task<List<CredentialResponse>> GetByUserAsync(Guid userId)
        {
            var result = new List<CredentialResponse>();

            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_GetByUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            CredentialResponse obj = new CredentialResponse();
            while (await reader.ReadAsync())
            {
                result.Add(MapCredentialResponse(reader));
            }

            return result;
        }

        // -------------------- GET DETAILS --------------------
        public async Task<CredentialResponse?> GetDetailsAsync(Guid credentialId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_GetDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@in_CredentialId", credentialId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCredentialResponse(reader);
            }

            return null;
        }

        // -------------------- ADD --------------------
        public async Task<Guid> AddAsync(Credential credential, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_Add", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            CredentialParameters(cmd, credential, userId);

            var outId = new SqlParameter("@out_CredentialId", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (Guid)outId.Value;
        }

        // -------------------- UPDATE --------------------
        public async Task<bool> UpdateAsync(Credential credential, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_CredentialId", credential.Id);
            CredentialParameters(cmd, credential, userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true;
        }

        // -------------------- DELETE (SOFT) --------------------
        public async Task<bool> DeleteAsync(Guid credentialId, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Credential_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_CredentialId", credentialId);
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true;
        }

        // -------------------- PARAMETERS --------------------
        private void CredentialParameters(SqlCommand cmd, Credential credential, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_UserId", userId);
            cmd.Parameters.AddWithValue("@in_SiteName", credential.SiteName);
            cmd.Parameters.AddWithValue("@in_SiteUrl", credential.SiteUrl);
            cmd.Parameters.AddWithValue("@in_Notes", credential.Notes);
            cmd.Parameters.AddWithValue("@in_UserName", credential.UserName);

            string password = credential.Password;

            string base64Key = AppSettings.AesMasterSecret;
            byte[] key = Convert.FromBase64String(base64Key);

            var result = HashAlgorithm.EncryptSecure(password, key);

            byte[] encryptedPassword = result.CipherText;
            byte[] storedIV = result.IV;
            string decryptedPassword = HashAlgorithm.DecryptSecure(encryptedPassword, key, storedIV);


            cmd.Parameters.Add("@in_EncryptedPassword", SqlDbType.VarBinary, -1).Value = encryptedPassword;   // ✅ byte[]
            cmd.Parameters.Add("@in_IV", SqlDbType.VarBinary, 16).Value = storedIV;       // ✅ byte[]
            //cmd.Parameters.AddWithValue("@in_EncryptedPassword", credential.Password);
        }

        // -------------------- MAPPER --------------------
        private static CredentialResponse MapCredentialResponse(SqlDataReader reader)
        {
            string base64Key = AppSettings.AesMasterSecret;
            byte[] key = Convert.FromBase64String(base64Key);
            var encryptedPassword = reader.GetFieldValue<byte[]>(reader.GetOrdinal("EncryptedPassword"));
            var storedIV = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Iv"));
            return new CredentialResponse
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),

                UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("UserId")),

                SiteName = reader.IsDBNull(reader.GetOrdinal("SiteName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SiteName")),

                SiteUrl = reader.IsDBNull(reader.GetOrdinal("SiteUrl")) ? null : reader.GetString(reader.GetOrdinal("SiteUrl")),

                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),

                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")),

                Password = HashAlgorithm.ApplyPattern(HashAlgorithm.DecryptSecure(encryptedPassword, key, storedIV))

                //IV = reader.GetDecimal(reader.GetOrdinal("IV")),
                //CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                //ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn"))
                //    ? null
                //    : reader.GetDateTime(reader.GetOrdinal("ModifiedOn"))
            };
        }
    }
}
