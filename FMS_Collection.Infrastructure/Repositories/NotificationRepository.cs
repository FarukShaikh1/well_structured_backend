using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public NotificationRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Notification>> GetAllAsync()
        {

            var Notifications = new List<Notification>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Notifications.Add(new Notification
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
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }

            return Notifications;
        }

        public async Task<List<NotificationListResponse>> GetNotificationListAsync(Guid userId)
        {
            var Notifications = new List<NotificationListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Notifications.Add(new NotificationListResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Id")),
                        CurrencyCoinType = reader.IsDBNull(reader.GetOrdinal("CurrencyCoinType")) ? null : reader.GetString(reader.GetOrdinal("CurrencyCoinType")),
                        CoinNoteName = reader.IsDBNull(reader.GetOrdinal("CoinNoteName")) ? null : reader.GetString(reader.GetOrdinal("CoinNoteName")),
                        CountryName = reader.IsDBNull(reader.GetOrdinal("CountryName")) ? null : reader.GetString(reader.GetOrdinal("CountryName")),
                        MetalsUsed = reader.IsDBNull(reader.GetOrdinal("MetalsUsed")) ? null : reader.GetString(reader.GetOrdinal("MetalsUsed")),
                        CoinWeightInGrams = reader.IsDBNull(reader.GetOrdinal("CoinWeightInGrams")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CoinWeightInGrams")),
                        ActualValue = reader.IsDBNull(reader.GetOrdinal("ActualValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("ActualValue")),
                        IndianValue = reader.IsDBNull(reader.GetOrdinal("IndianValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("IndianValue")),
                        PrintedYear = reader.IsDBNull(reader.GetOrdinal("PrintedYear")) ? null : reader.GetString(reader.GetOrdinal("PrintedYear")),
                        DiameterOfCoin = reader.IsDBNull(reader.GetOrdinal("DiameterOfCoin")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("DiameterOfCoin")),
                        LengthOfNote = reader.IsDBNull(reader.GetOrdinal("LengthOfNote")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("LengthOfNote")),
                        BreadthOfNote = reader.IsDBNull(reader.GetOrdinal("BreadthOfNote")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("BreadthOfNote")),
                        Speciality = reader.IsDBNull(reader.GetOrdinal("Speciality")) ? null : reader.GetString(reader.GetOrdinal("Speciality")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        CurrencySymbol = reader.IsDBNull(reader.GetOrdinal("CurrencySymbol")) ? null : reader.GetString(reader.GetOrdinal("CurrencySymbol")),
                        RupeeSymbol = reader.IsDBNull(reader.GetOrdinal("RupeeSymbol")) ? null : reader.GetString(reader.GetOrdinal("RupeeSymbol")),
                        IsVerified = reader.IsDBNull(reader.GetOrdinal("IsVerified")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsVerified"))
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }

            return Notifications;
        }

        public async Task<List<NotificationSummaryResponse>> GetSummaryAsync()
        {
            var Notifications = new List<NotificationSummaryResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("NotificationSummary_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Notifications.Add(new NotificationSummaryResponse
                    {
                        CountryId = reader["CountryId"] != DBNull.Value ? Convert.ToInt32(reader["CountryId"]) : 0,
                        CountryName = reader["CountryName"]?.ToString(),
                        CurrencyCode = reader["CurrencyCode"]?.ToString(),
                        CurrencyName = reader["CurrencyName"]?.ToString(),
                        CurrencySymbol = reader["CurrencySymbol"]?.ToString(),
                        NumberOfNotes = reader["NumberOfNotes"] != DBNull.Value ? Convert.ToInt32(reader["NumberOfNotes"]) : (int?)null,
                        NumberOfCoins = reader["NumberOfCoins"] != DBNull.Value ? Convert.ToInt32(reader["NumberOfCoins"]) : (int?)null,
                        Total = reader["Total"] != DBNull.Value ? Convert.ToInt32(reader["Total"]) : (int?)null
                        // No need to set Total manually if it's a computed property
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }

            return Notifications;
        }

        public async Task<NotificationDetailsResponse> GetNotificationDetailsAsync(Guid NotificationId, Guid userId)
        {
            var result = new NotificationDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_NotificationId", SqlDbType.UniqueIdentifier) { Value = NotificationId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new NotificationDetailsResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("Id")),
                        CollectionCurrencyTypeId = reader.GetGuid(reader.GetOrdinal("CollectionCurrencyTypeId")),
                        CoinNoteName = reader.IsDBNull(reader.GetOrdinal("CoinNoteName")) ? null : reader.GetString(reader.GetOrdinal("CoinNoteName")),
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
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        AssetType = reader.IsDBNull(reader.GetOrdinal("AssetType")) ? null : reader.GetString(reader.GetOrdinal("AssetType")),
                        ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                        ThumbnailPath = reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPath")),
                        CurrencySymbol = reader.IsDBNull(reader.GetOrdinal("CurrencySymbol")) ? null : reader.GetString(reader.GetOrdinal("CurrencySymbol")),
                        RupeeSymbol = reader.IsDBNull(reader.GetOrdinal("RupeeSymbol")) ? null : reader.GetString(reader.GetOrdinal("RupeeSymbol")),
                        IsVerified = reader.IsDBNull(reader.GetOrdinal("IsVerified")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsVerified"))

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
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }

            return result;
        }

        public async Task<Guid> AddAsync(NotificationRequest Notification, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                NotificationRequestParameters(cmd, Notification, userId);
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
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }
        }

        public async Task<bool> UpdateAsync(NotificationRequest Notification, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_NotificationId", Notification.Id);

                NotificationRequestParameters(cmd, Notification, userId);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred. actual error is :{ex}", ex);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Guid NotificationId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Notification_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_NotificationId", NotificationId);
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

        private void NotificationRequestParameters(SqlCommand cmd, NotificationRequest Notification, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_CoinNoteName", (object?)Notification.CoinNoteName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_CollectionCoinTypeId", (object?)Notification.CollectionCoinTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_CountryId", (object?)Notification.CountryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_MetalsUsed", (object?)Notification.MetalsUsed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_CoinWeightInGrams", (object?)Notification.CoinWeightInGrams ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_ActualValue", (object?)Notification.ActualValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_IndianValue", (object?)Notification.IndianValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_PrintedYear", (object?)Notification.PrintedYear ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_Speciality", (object?)Notification.Speciality ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_DiameterOfCoin", (object?)Notification.DiameterOfCoin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_LengthOfNote", (object?)Notification.LengthOfNote ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_BreadthOfNote", (object?)Notification.BreadthOfNote ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_Description", (object?)Notification.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_AssetId", (object?)Notification.AssetId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_UserId", userId);
        }
    }
}
