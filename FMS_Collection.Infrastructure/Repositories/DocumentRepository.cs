using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public DocumentRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Document>> GetAllAsync()
        {

            var Documents = new List<Document>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Documents.Add(new Document
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CoinNoteName = reader.GetString(reader.GetOrdinal("CoinNoteName")),
                        CurrencyTypeId = reader.IsDBNull(reader.GetOrdinal("CurrencyTypeId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CurrencyTypeId")),
                        CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CountryId")),
                        MetalsUsed = reader.IsDBNull(reader.GetOrdinal("MetalsUsed")) ? null : reader.GetString(reader.GetOrdinal("MetalsUsed")),
                        CoinWeightInGrams = reader.IsDBNull(reader.GetOrdinal("CoinWeightInGrams")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CoinWeightInGrams")),
                        ActualValue = reader.IsDBNull(reader.GetOrdinal("ActualValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("ActualValue")),
                        IndianValue = reader.IsDBNull(reader.GetOrdinal("IndianValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("IndianValue")),
                        PrintedYear = reader.IsDBNull(reader.GetOrdinal("PrintedYear")) ? null : reader.GetString(reader.GetOrdinal("PrintedYear")),
                        Speciality = reader.IsDBNull(reader.GetOrdinal("Speciality")) ? null : reader.GetString(reader.GetOrdinal("Speciality")),
                        DiameterOfCoin = reader.IsDBNull(reader.GetOrdinal("DiameterOfCoin")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("DiameterOfCoin")),
                        LengthOfNote = reader.IsDBNull(reader.GetOrdinal("LengthOfNote")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("LengthOfNote")),
                        BreadthOfNote = reader.IsDBNull(reader.GetOrdinal("BreadthOfNote")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("BreadthOfNote")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        AssetId = reader.GetGuid(reader.GetOrdinal("AssetId")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        IsVerified = reader.IsDBNull(reader.GetOrdinal("IsVerified")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsVerified"))
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Documents;
        }

        public async Task<List<DocumentListResponse>> GetDocumentListAsync(Guid userId)
        {
            var Documents = new List<DocumentListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Documents.Add(new DocumentListResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Id")),
                        DocumentName = reader.IsDBNull(reader.GetOrdinal("DocumentName")) ? null : reader.GetString(reader.GetOrdinal("DocumentName")),
                        Keywords = reader.IsDBNull(reader.GetOrdinal("Keywords")) ? null : reader.GetString(reader.GetOrdinal("Keywords")),
                        OriginalPath = reader.IsDBNull(reader.GetOrdinal("OriginalPath")) ? null : reader.GetString(reader.GetOrdinal("OriginalPath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        PreviewPath = reader.IsDBNull(reader.GetOrdinal("PreviewPath")) ? null : reader.GetString(reader.GetOrdinal("PreviewPath"))
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Documents;
        }

        public async Task<DocumentDetailsResponse> GetDocumentDetailsAsync(Guid DocumentId, Guid userId)
        {
            var result = new DocumentDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_DocumentId", SqlDbType.UniqueIdentifier) { Value = DocumentId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new DocumentDetailsResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Id")),
                        DocumentName = reader.IsDBNull(reader.GetOrdinal("DocumentName")) ? null : reader.GetString(reader.GetOrdinal("DocumentName")),
                        Keywords = reader.IsDBNull(reader.GetOrdinal("Keywords")) ? null : reader.GetString(reader.GetOrdinal("Keywords")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        AssetType = reader.IsDBNull(reader.GetOrdinal("AssetType")) ? null : reader.GetString(reader.GetOrdinal("AssetType")),
                        OriginalPath = reader.IsDBNull(reader.GetOrdinal("OriginalPath")) ? null : reader.GetString(reader.GetOrdinal("OriginalPath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        PreviewPath = reader.IsDBNull(reader.GetOrdinal("PreviewPath")) ? null : reader.GetString(reader.GetOrdinal("PreviewPath")),
                            
                        //// Base class (CommonResponse) properties:
                        //IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        //CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        //ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        //CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        //ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy"))
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return result;
        }

        public async Task<Guid> AddAsync(DocumentRequest Document, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                DocumentRequestParameters(cmd, Document, userId);
                // Add Output Parameter
                var outIdParam = new SqlParameter("@out_Id", SqlDbType.UniqueIdentifier)
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<bool> UpdateAsync(DocumentRequest Document, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_DocumentId", Document.Id);

                DocumentRequestParameters(cmd, Document, userId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Guid DocumentId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Document_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_DocumentId", DocumentId);
                cmd.Parameters.AddWithValue("@in_UserId", userId);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.GetBoolean("IsSuccess");
                }
            }
            catch(Exception ex)
            {

            }
            return false;
        }

        private void DocumentRequestParameters(SqlCommand cmd, DocumentRequest Document, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_DocumentName", (object?)Document.DocumentName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_Keywords", (object?)Document.Keywords ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_AssetId", (object?)Document.AssetId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_UserId", userId);
        }
    }
}
