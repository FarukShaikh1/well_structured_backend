using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class CommonListRepository : ICommonListRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public CommonListRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<CommonList>> GetAllCommonListAsync()
        {
            var Commons = new List<CommonList>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonList_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Commons.Add(new CommonList
                    {
                        CommonListId = reader.GetGuid(reader.GetOrdinal("CommonListId")),
                        CommonListName = reader.GetString(reader.GetOrdinal("CommonListName")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        DependantOn = reader.IsDBNull(reader.GetOrdinal("DependantOn")) ? null : reader.GetString(reader.GetOrdinal("DependantOn"))
                    });
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Commons.", ex);
            }

            return Commons;
        }

        public async Task<List<CommonListItem>> GetAllCommonListItemAsync()
        {
            var Commons = new List<CommonListItem>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Commons.Add(new CommonListItem
                    {
                        CommonListItemId = reader.GetGuid(reader.GetOrdinal("CommonListItemId")),
                        CommonListId = reader.GetGuid(reader.GetOrdinal("CommonListId")),
                        ListItemName = reader.GetString(reader.GetOrdinal("ListItemName")),
                        SequenceNumber = reader.IsDBNull(reader.GetOrdinal("SequenceNumber")) ? 0 : reader.GetInt16(reader.GetOrdinal("SequenceNumber")),
                        ListItemDescription = reader.IsDBNull(reader.GetOrdinal("ListItemDescription")) ? null : reader.GetString(reader.GetOrdinal("ListItemDescription")),
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
                throw new Exception("An error occurred while retrieving Commons.", ex);
            }

            return Commons;
        }

        public async Task<List<CommonListResponse>> GetCommonListAsync()
        {
            var Commons = new List<CommonListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonList_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Commons.Add(new CommonListResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CommonListId")),
                        CommonListName = reader.GetString(reader.GetOrdinal("CommonListName")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        DependantOn = reader.IsDBNull(reader.GetOrdinal("DependantOn")) ? null : reader.GetString(reader.GetOrdinal("DependantOn"))

                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Commons.", ex);
            }

            return Commons;
        }

        public async Task<List<CommonListItemResponse>> GetCommonListItemAsync(Guid CommonListId)
        {
            var Commons = new List<CommonListItemResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_CommonListId", SqlDbType.UniqueIdentifier) { Value = CommonListId });
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Commons.Add(new CommonListItemResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CommonListName = reader.GetString(reader.GetOrdinal("CommonListName")),
                        ListItemName = reader.GetString(reader.GetOrdinal("ListItemName")),
                        ListItemDescription = reader.IsDBNull(reader.GetOrdinal("ListItemDescription"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("ListItemDescription")),
                        SequenceNumber = reader.IsDBNull(reader.GetOrdinal("SequenceNumber")) ? 0 : reader.GetInt32(reader.GetOrdinal("SequenceNumber")),
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Commons.", ex);
            }

            return Commons;
        }

        public async Task<CommonListResponse> GetCommonListDetailsAsync(Guid CommonListId)
        {
            var result = new CommonListResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_CommonListItemId", SqlDbType.UniqueIdentifier) { Value = CommonListId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new CommonListResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CommonListId")),
                        CommonListName = reader.GetString(reader.GetOrdinal("CommonListName")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                        DependantOn = reader.IsDBNull(reader.GetOrdinal("DependantOn")) ? null : reader.GetString(reader.GetOrdinal("DependantOn"))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Common details.", ex);
            }

            return result;
        }

        public async Task<CommonListItemResponse> GetCommonListItemDetailsAsync(Guid CommonListId)
        {
            var result = new CommonListItemResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                 cmd.Parameters.Add(new SqlParameter("@in_CommonListItemId", SqlDbType.UniqueIdentifier) { Value = CommonListId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new CommonListItemResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CommonListItemId")),
                        CommonListName = reader.GetString(reader.GetOrdinal("CommonListName")),
                        ListItemName = reader.GetString(reader.GetOrdinal("ListItemName")),
                        ListItemDescription = reader.IsDBNull(reader.GetOrdinal("ListItemDescription"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("ListItemDescription"))
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Common details.", ex);
            }

            return result;
        }

        public async Task AddCommonListAsync(CommonListRequest Common, Guid CommonListId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonList_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddCommonListRequestParameters(cmd, Common, CommonListId);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the Common.", ex);
            }
        }

        public async Task AddCommonListItemAsync(CommonListItemRequest Common, Guid CommonListId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddCommonListItemRequestParameters(cmd, Common, CommonListId);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the Common.", ex);
            }
        }

        public async Task UpdateCommonListAsync(CommonListRequest Common, Guid CommonListId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonList_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddCommonListRequestParameters(cmd, Common, CommonListId);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Common.", ex);
            }
        }

        public async Task UpdateCommonListItemAsync(CommonListItemRequest Common, Guid CommonListId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CommonListItem_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddCommonListItemRequestParameters(cmd, Common, CommonListId);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Common.", ex);
            }
        }

        public async Task<bool> DeleteCommonListAsync(Guid CommonListId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Common_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@in_CommonListId", SqlDbType.UniqueIdentifier) { Value = CommonListId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Common.", ex);
            }
            return true;
        }

        public async Task<bool> DeleteCommonListItemAsync(Guid CommonListItemId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Common_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@in_CommonListId", SqlDbType.UniqueIdentifier) { Value = CommonListItemId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Common.", ex);
            }
            return true;
        }

        public async Task<List<CountryWithCurrency>> GetCountryListAsync()
        {
            var Commons = new List<CountryWithCurrency>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("CountryList_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Commons.Add(new CountryWithCurrency
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("Id")),
                        Country = reader.GetString(reader.GetOrdinal("CountryName")),
                        Currency = reader.GetString(reader.GetOrdinal("CurrencyName")),
                        Code = reader.GetString(reader.GetOrdinal("CurrencyCode")),
                        Symbol = reader.GetString(reader.GetOrdinal("CurrencySymbol")),
                        CountryCode = reader.IsDBNull(reader.GetOrdinal("CountryCode")) ? null : reader.GetString(reader.GetOrdinal("CountryCode")),
                        DialCode = reader.IsDBNull(reader.GetOrdinal("DialCode")) ? null : reader.GetInt32(reader.GetOrdinal("DialCode")),
                        DisplaySequence = reader.IsDBNull(reader.GetOrdinal("DisplaySequence")) ? null : reader.GetInt32(reader.GetOrdinal("DisplaySequence")),
                        Nationality = reader.IsDBNull(reader.GetOrdinal("Nationality")) ? null : reader.GetString(reader.GetOrdinal("Nationality"))
                        //CreatedOn = reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                        //CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                        //ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedOn")),
                        //ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetGuid(reader.GetOrdinal("ModifiedBy")),
                        //IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Commons.", ex);
            }

            return Commons;
        }


        private void AddCommonListRequestParameters(SqlCommand cmd, CommonListRequest Common, Guid CommonListId)
        {

        }
        private void AddCommonListItemRequestParameters(SqlCommand cmd, CommonListItemRequest Common, Guid CommonListId)
        {

        }
    }
}
