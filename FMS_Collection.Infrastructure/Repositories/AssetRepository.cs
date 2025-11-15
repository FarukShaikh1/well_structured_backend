using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public AssetRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Asset>> GetAllAsync()
        {

            var assets = new List<Asset>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Asset_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    assets.Add(new Asset
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        AssetType = reader.GetString(reader.GetOrdinal("AssetType")),
                        UploadedFileName = reader.GetString(reader.GetOrdinal("UploadedFileName")),
                        OriginalPath = reader.GetString(reader.GetOrdinal("OriginalPath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        PreviewPath = reader.IsDBNull(reader.GetOrdinal("PreviewPath")) ? null : reader.GetString(reader.GetOrdinal("PreviewPath")),
                        ContentType = reader.IsDBNull(reader.GetOrdinal("ContentType")) ? null : reader.GetString(reader.GetOrdinal("ContentType")),
                        IsNonSecuredFile = reader.IsDBNull(reader.GetOrdinal("IsNonSecuredFile")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsNonSecuredFile")),
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
                throw new Exception(FMS_Collection.Core.Constants.Constants.Messages.RetrieveAssetsError, ex);
            }

            return assets;
        }

        public async Task<AssetResponse> GetAssetDetailsAsync(Guid? assetId)
        {
            var result = new AssetResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Asset_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_AssetId", SqlDbType.UniqueIdentifier) { Value = assetId });

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new AssetResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        AssetType = reader.IsDBNull(reader.GetOrdinal("AssetType")) ? null : reader.GetString(reader.GetOrdinal("AssetType")),
                        UploadedFileName = reader.IsDBNull(reader.GetOrdinal("UploadedFileName")) ? null : reader.GetString(reader.GetOrdinal("UploadedFileName")),
                        OriginalPath = reader.IsDBNull(reader.GetOrdinal("OriginalPath")) ? null : reader.GetString(reader.GetOrdinal("OriginalPath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        PreviewPath = reader.IsDBNull(reader.GetOrdinal("PreviewPath")) ? null : reader.GetString(reader.GetOrdinal("PreviewPath")),
                        ContentType = reader.IsDBNull(reader.GetOrdinal("ContentType")) ? null : reader.GetString(reader.GetOrdinal("ContentType")),
                        IsNonSecuredFile = reader.IsDBNull(reader.GetOrdinal("IsNonSecuredFile")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsNonSecuredFile"))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(FMS_Collection.Core.Constants.Constants.Messages.RetrieveAssetDetailsError, ex);
            }

            return result;
        }

        public async Task<Guid> AddAsync(AssetRequest asset, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Asset_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AssetRequestParameters(cmd, asset, userId);
                // Add Output Parameter
                var outIdParam = new SqlParameter("@out_AssetId", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outIdParam);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                // Retrieve the Output Parameter Value
                Guid newInsertedId = (Guid)(outIdParam.Value ?? Guid.Empty);
                return newInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception(FMS_Collection.Core.Constants.Constants.Messages.AddAssetError, ex);
            }
        }

        public async Task UpdateAsync(AssetRequest asset, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Asset_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_AssetId", (object?)asset.Id ?? DBNull.Value);
                AssetRequestParameters(cmd, asset, userId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(FMS_Collection.Core.Constants.Constants.Messages.UpdateAssetError, ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid? AssetId, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Asset_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_AssetId", AssetId);
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean("IsSuccess");
            }
            return false;
        }


        private void AssetRequestParameters(SqlCommand cmd, AssetRequest asset, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_UserId", userId);
            cmd.Parameters.AddWithValue("@in_AssetType", (object?)asset.AssetType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_UploadedFileName", (object?)asset.UploadedFileName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_OriginalPath", (object?)asset.OriginalPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_ThumbnailPath", (object?)asset.ThumbnailPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_PreviewPath", (object?)asset.PreviewPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_ContentType", (object?)asset.ContentType ?? DBNull.Value);
        }

    }
}
