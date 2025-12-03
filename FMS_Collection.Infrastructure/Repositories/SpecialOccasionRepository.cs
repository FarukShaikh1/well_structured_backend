using Azure.Core;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class SpecialOccasionRepository : ISpecialOccasionRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public SpecialOccasionRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<SpecialOccasion>> GetAllAsync()
        {
            var days = new List<SpecialOccasion>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    days.Add(new SpecialOccasion
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        specialOccasionDate = reader.IsDBNull(reader.GetOrdinal("specialOccasionDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("specialOccasionDate")),
                        PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                        SuperAdminRelationId = reader.IsDBNull(reader.GetOrdinal("SuperAdminRelationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SuperAdminRelationId")),
                        MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? null : reader.GetString(reader.GetOrdinal("ContactNumber")),
                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString(reader.GetOrdinal("EmailId")),
                        DayTypeId = reader.IsDBNull(reader.GetOrdinal("DayTypeId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("DayTypeId")),
                        DayTypeName = reader.IsDBNull(reader.GetOrdinal("DayTypeName")) ? null : reader.GetString(reader.GetOrdinal("DayTypeName")),
                        RelationName = reader.IsDBNull(reader.GetOrdinal("RelationName")) ? null : reader.GetString(reader.GetOrdinal("RelationName")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? (char?)null : Convert.ToChar(reader.GetString(reader.GetOrdinal("Gender"))),
                        RelationId = reader.IsDBNull(reader.GetOrdinal("RelationId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("RelationId")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        IsRestricted = reader.IsDBNull(reader.GetOrdinal("IsRestricted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsRestricted")),
                        IsVerified = reader.IsDBNull(reader.GetOrdinal("IsVerified")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsVerified"))
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return days;
        }

        public async Task<List<SpecialOccasionListResponse>> GetDayListAsync(Guid userId)
        {
            var days = new List<SpecialOccasionListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    days.Add(new SpecialOccasionListResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        SpecialOccasionDate = reader.IsDBNull(reader.GetOrdinal("specialOccasionDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("specialOccasionDate")),
                        PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString(reader.GetOrdinal("EmailId")),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                        ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        RelationName = reader.IsDBNull(reader.GetOrdinal("RelationName")) ? null : reader.GetString(reader.GetOrdinal("RelationName")),
                        DayType = reader.IsDBNull(reader.GetOrdinal("DayType")) ? null : reader.GetString(reader.GetOrdinal("DayType")),
                        MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber"))

                        // Base class (CommonResponse) properties:
                        //IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        //CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        //ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        //CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        //ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy"))
                    });
                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return days;
        }

        public async Task<SpecialOccasionDetailsResponse> GetDayDetailsAsync(Guid dayId, Guid userId)
        {
            var result = new SpecialOccasionDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_SpecialOccasionId", SqlDbType.UniqueIdentifier) { Value = dayId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new SpecialOccasionDetailsResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Id")),
                        SpecialOccasionDate = reader.IsDBNull(reader.GetOrdinal("specialOccasionDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("specialOccasionDate")),
                        PersonName = reader.IsDBNull(reader.GetOrdinal("PersonName")) ? null : reader.GetString(reader.GetOrdinal("PersonName")),
                        DayTypeId = reader.IsDBNull(reader.GetOrdinal("DayTypeId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("DayTypeId")),
                        RelationId = reader.IsDBNull(reader.GetOrdinal("RelationId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("RelationId")),
                        MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? null : reader.GetString(reader.GetOrdinal("ContactNumber")),
                        EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? null : reader.GetString(reader.GetOrdinal("EmailId")),
                        Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        DayTypeName = reader.IsDBNull(reader.GetOrdinal("DayTypeName")) ? null : reader.GetString(reader.GetOrdinal("DayTypeName")),
                        RelationName = reader.IsDBNull(reader.GetOrdinal("RelationName")) ? null : reader.GetString(reader.GetOrdinal("RelationName")),
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return result;
        }

        public async Task<SpecialOccasionDetailsResponse> AddAsync(SpecialOccasionRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                AddDayRequestParameters(cmd, request, userId);

                await conn.OpenAsync();

                // SP returns a full SELECT resultset from SpecialOccasion_Details_Get
                using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.Read())
                {
                    return new SpecialOccasionDetailsResponse
                    {
                    };
                }
                var response = getResponse(reader);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual,
                        ex.Message),
                    ex);
            }
        }

        public async Task<SpecialOccasionDetailsResponse> UpdateAsync(SpecialOccasionRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_specialOccasionId", request.Id);
                AddDayRequestParameters(cmd, request, userId);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.Read())
                {
                    return new SpecialOccasionDetailsResponse
                    {
                    };
                }
                var response = getResponse(reader);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid specialoccasionId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_SpecialOccasionId", specialoccasionId);
                cmd.Parameters.AddWithValue("@in_UserId", userId);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return reader.GetBoolean("IsSuccess");
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void AddDayRequestParameters(SqlCommand cmd, SpecialOccasionRequest request, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_specialOccasionDate", request.SpecialOccasionDate);
            cmd.Parameters.AddWithValue("@in_PersonName", request.PersonName);
            cmd.Parameters.AddWithValue("@in_Address", (object?)request.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@In_ContactNumber", (object?)request.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@In_MobileNumber", (object?)request.MobileNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_EmailId", (object?)request.EmailId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_Gender", request.Gender);
            cmd.Parameters.AddWithValue("@In_DayTypeId", request.OccasionTypeId);
            cmd.Parameters.AddWithValue("@In_RelationId", request.RelationId);
            cmd.Parameters.AddWithValue("@in_UserId", userId);
            cmd.Parameters.AddWithValue("@in_assetId", (object?)request.AssetId ?? DBNull.Value);
        }

        private SpecialOccasionDetailsResponse getResponse(SqlDataReader reader)
        {
            return new SpecialOccasionDetailsResponse
            {
                Id = reader["Id"] as Guid?,
                SpecialOccasionDate = reader["SpecialOccasionDate"] as DateTime?,
                PersonName = reader["PersonName"] as string,
                Address = reader["Address"] as string,
                RelationId = reader["RelationId"] as Guid?,
                RelationName = reader["RelationName"] as string,
                ContactNumber = reader["ContactNumber"] as string,
                MobileNumber = reader["MobileNumber"] as string,
                EmailId = reader["EmailId"] as string,
                Gender = reader["Gender"] as string,
                DayTypeId = reader["DayTypeId"] as Guid?,
                DayTypeName = reader["DayTypeName"] as string,
                AssetId = reader["AssetId"] as Guid?,
                // Additional returned fields
                CreatedOn = reader["CreatedOn"] as DateTime?,
                CreatedByName = reader["CreatedBy"] as string,
                ModifiedOn = reader["ModifiedOn"] as DateTime?,
                ModifiedByName = reader["ModifiedBy"] as string,
            };
        }
    }
}
