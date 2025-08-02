using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public TransactionRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            var Transactions = new List<Transaction>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Transactions.Add(new Transaction
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        AccountId = reader["AccountId"] != DBNull.Value ? (Guid?)reader["AccountId"] : null,
                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? (DateTime?)reader["TransactionDate"] : null,
                        SourceOrReason = reader["SourceOrReason"]?.ToString(),
                        Amount = reader["Amount"] != DBNull.Value ? (decimal?)reader["Amount"] : null,
                        Balance = reader["Balance"] != DBNull.Value ? (decimal?)reader["Balance"] : null,
                        Category = reader["Category"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        AssetId = reader["AssetId"] != DBNull.Value ? (Guid?)reader["AssetId"] : null,
                        Purpose = reader["Purpose"]?.ToString(),
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? (DateTime?)reader["CreatedOn"] : null,
                        CreatedBy = reader["CreatedBy"] != DBNull.Value ? (Guid?)reader["CreatedBy"] : null,
                        ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)reader["ModifiedOn"] : null,
                        ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? (Guid?)reader["ModifiedBy"] : null,
                        IsDeleted = reader["IsDeleted"] != DBNull.Value ? (bool?)reader["IsDeleted"] : null
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transactions.", ex);
            }

            return Transactions;
        }

        public async Task<List<TransactionListResponse>> GetTransactionListAsync(TransactionFilterRequest filter, Guid userId)
        {
            var Transactions = new List<TransactionListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_FromDate", SqlDbType.Date) { Value = filter.FromDate });
                cmd.Parameters.Add(new SqlParameter("@in_ToDate", SqlDbType.Date) { Value = filter.ToDate });
                cmd.Parameters.Add(new SqlParameter("@in_SourceOrReason", SqlDbType.VarChar) { Value = filter.SourceOrReason });
                cmd.Parameters.Add(new SqlParameter("@in_MinAmount", SqlDbType.Decimal) { Value = filter.MinAmount });
                cmd.Parameters.Add(new SqlParameter("@in_MaxAmount", SqlDbType.Decimal) { Value = filter.MaxAmount });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Transactions.Add(new TransactionListResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        AccountId = reader["AccountId"] != DBNull.Value ? (Guid?)reader["AccountId"] : null,
                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? (DateTime?)reader["TransactionDate"] : null,
                        SourceOrReason = reader["SourceOrReason"]?.ToString(),
                        Income = reader["Income"] != DBNull.Value ? (decimal?)reader["Income"] : null,
                        Expense = reader["Expense"] != DBNull.Value ? (decimal?)reader["Expense"] : null,
                        Balance = reader["Balance"] != DBNull.Value ? (decimal?)reader["Balance"] : null,
                        Description = reader["Description"]?.ToString(),
                        AssetId = reader["AssetId"] != DBNull.Value ? (Guid?)reader["AssetId"] : null,
                        Purpose = reader["Purpose"]?.ToString(),
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? (DateTime?)reader["CreatedOn"] : null,
                        CreatedBy = reader["CreatedBy"] != DBNull.Value ? (Guid?)reader["CreatedBy"] : null,
                        ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)reader["ModifiedOn"] : null,
                        ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? (Guid?)reader["ModifiedBy"] : null,
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transactions.", ex);
            }

            return Transactions;
        }


        public async Task<List<TransactionSummaryResponse>> GetTransactionSummaryAsync(TransactionFilterRequest filter, Guid userId)
        {
            var Transactions = new List<TransactionSummaryResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("TransactionSummary_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_FromDate", SqlDbType.Date) { Value = filter.FromDate });
                cmd.Parameters.Add(new SqlParameter("@in_ToDate", SqlDbType.Date) { Value = filter.ToDate });
                cmd.Parameters.Add(new SqlParameter("@in_SourceOrReason", SqlDbType.VarChar) { Value = filter.SourceOrReason });
                cmd.Parameters.Add(new SqlParameter("@in_MinAmount", SqlDbType.Decimal) { Value = filter.MinAmount });
                cmd.Parameters.Add(new SqlParameter("@in_MaxAmount", SqlDbType.Decimal) { Value = filter.MaxAmount });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Transactions.Add(new TransactionSummaryResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Cash = reader.IsDBNull(reader.GetOrdinal("Cash")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Cash")),
                        Balance = reader.IsDBNull(reader.GetOrdinal("Balance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Balance")),

                    });
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transactions.", ex);
            }

            return Transactions;
        }

        public async Task<List<TransactionReportResponse>> GetTransactionReportAsync(TransactionFilterRequest filter, Guid userId)
        {
            var Transactions = new List<TransactionReportResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("TransactionReport_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                cmd.Parameters.Add(new SqlParameter("@in_FromDate", SqlDbType.Date) { Value = filter.FromDate });
                cmd.Parameters.Add(new SqlParameter("@in_ToDate", SqlDbType.Date) { Value = filter.ToDate });
                cmd.Parameters.Add(new SqlParameter("@in_SourceOrReason", SqlDbType.VarChar) { Value = filter.SourceOrReason });
                cmd.Parameters.Add(new SqlParameter("@in_MinAmount", SqlDbType.Decimal) { Value = filter.MinAmount });
                cmd.Parameters.Add(new SqlParameter("@in_MaxAmount", SqlDbType.Decimal) { Value = filter.MaxAmount });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Transactions.Add(new TransactionReportResponse
                    {
                        FirstDate = reader["FirstDate"] != DBNull.Value ? Convert.ToDateTime(reader["FirstDate"]) : DateTime.MinValue,
                        LastDate = reader["LastDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastDate"]) : DateTime.MinValue,
                        SourceOrReason = reader["SourceOrReason"] != DBNull.Value ? reader["SourceOrReason"].ToString() : null,
                        Description = reader["Descriptions"] != DBNull.Value ? reader["Descriptions"].ToString() : null,
                        TakenAmount = reader["TakenAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TakenAmount"]) : (decimal?)null,
                        GivenAmount = reader["GivenAmount"] != DBNull.Value ? Convert.ToDecimal(reader["GivenAmount"]) : (decimal?)null,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : (decimal?)null,
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transactions.", ex);
            }

            return Transactions;
        }

        public async Task<TransactionDetailsResponse> GetTransactionDetailsAsync(Guid TransactionId, Guid userId)
        {
            var result = new TransactionDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_TransactionId", SqlDbType.UniqueIdentifier) { Value = TransactionId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new TransactionDetailsResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? null : reader.GetGuid(reader.GetOrdinal("Id")),
                        TransactionDate = reader.IsDBNull(reader.GetOrdinal("TransactionDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Cash = reader.IsDBNull(reader.GetOrdinal("Cash")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Cash")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transaction details.", ex);
            }

            return result;
        }

        public async Task<TransactionSuggestionList> GetTransactionSuggestionListAsync(Guid userId)
        {
            var result = new TransactionSuggestionList();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("ExpenseSuggestionList_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new TransactionSuggestionList
                    {
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving Transaction details.", ex);
            }

            return result;
        }

        public async Task<Guid>? AddAsync(TransactionRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add Input Parameters (matching your class)
                cmd.Parameters.AddWithValue("@in_AccountId", request.AccountId);
                cmd.Parameters.AddWithValue("@in_TransactionDate", request.TransactionDate);
                cmd.Parameters.AddWithValue("@in_SourceOrReason", request.SourceOrReason);
                cmd.Parameters.AddWithValue("@in_Amount", request.Amount);
                cmd.Parameters.AddWithValue("@in_Category", request.Category);
                cmd.Parameters.AddWithValue("@in_Description", request.Description);
                cmd.Parameters.AddWithValue("@in_Purpose", request.Purpose);
                cmd.Parameters.AddWithValue("@in_UserId", userId);

                // Add Output Parameter
                var outIdParam = new SqlParameter("@out_TransactionId", SqlDbType.UniqueIdentifier)
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
                throw new Exception("An error occurred while adding the special occasion.", ex);
            }
        }

        public async Task<bool> UpdateAsync(TransactionRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_TransactionId", request.Id);
                cmd.Parameters.AddWithValue("@in_TransactionDate", request.TransactionDate);
                cmd.Parameters.AddWithValue("@in_SourceOrReason", request.SourceOrReason);
                cmd.Parameters.AddWithValue("@in_Purpose", request.Purpose);
                cmd.Parameters.AddWithValue("@in_Amount", request.Amount);
                cmd.Parameters.AddWithValue("@in_Category", request.Category);
                cmd.Parameters.AddWithValue("@in_Description", request.Description);
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

        public async Task<bool> DeleteAsync(Guid TransactionId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Transaction_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_TransactionId", TransactionId);
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
    }
}
