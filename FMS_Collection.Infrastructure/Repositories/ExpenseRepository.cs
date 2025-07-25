using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ExpenseRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            var expenses = new List<Expense>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    expenses.Add(new Expense
                    {
                        ExpenseId = reader.GetGuid(reader.GetOrdinal("ExpenseId")),
                        ExpenseDate = reader.IsDBNull(reader.GetOrdinal("ExpenseDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpenseDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Cash = reader.IsDBNull(reader.GetOrdinal("Cash")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Cash")),
                        SBIAccount = reader.IsDBNull(reader.GetOrdinal("SBIAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SBIAccount")),
                        CBIAccount = reader.IsDBNull(reader.GetOrdinal("CBIAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CBIAccount")),
                        Other = reader.IsDBNull(reader.GetOrdinal("Other")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Other")),
                        TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                        CashBalance = reader.IsDBNull(reader.GetOrdinal("CashBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CashBalance")),
                        SBIBalance = reader.IsDBNull(reader.GetOrdinal("SBIBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SBIBalance")),
                        CBIBalance = reader.IsDBNull(reader.GetOrdinal("CBIBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CBIBalance")),
                        OtherBalance = reader.IsDBNull(reader.GetOrdinal("OtherBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OtherBalance")),
                        TotalAvailable = reader.IsDBNull(reader.GetOrdinal("TotalAvailable")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAvailable")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        ReferenceNumber = reader.IsDBNull(reader.GetOrdinal("ReferenceNumber")) ? null : reader.GetString(reader.GetOrdinal("ReferenceNumber")),
                        IsInvoiceAvailable = reader.IsDBNull(reader.GetOrdinal("IsInvoiceAvailable")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsInvoiceAvailable")),
                        TransactionStatusId = reader.IsDBNull(reader.GetOrdinal("TransactionStatusId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TransactionStatusId")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
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
                throw new Exception("An error occurred while retrieving expenses.", ex);
            }

            return expenses;
        }

        public async Task<List<ExpenseListResponse>> GetExpenseListAsync(ExpenseFilterRequest filter, Guid userId)
        {
            var expenses = new List<ExpenseListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_Get", conn)
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
                    expenses.Add(new ExpenseListResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        ExpenseDate = reader.GetDateTime(reader.GetOrdinal("ExpenseDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        ModeOfTransaction = reader.IsDBNull(reader.GetOrdinal("ModeOfTransaction")) ? null : reader.GetString(reader.GetOrdinal("ModeOfTransaction")),
                        Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Amount")),
                        Credit = reader.IsDBNull(reader.GetOrdinal("Credit")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Credit")),
                        Debit = reader.IsDBNull(reader.GetOrdinal("Debit")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Debit"))

                        // Mapping base class (CommonResponse) properties:
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
                throw new Exception("An error occurred while retrieving expenses.", ex);
            }

            return expenses;
        }


        public async Task<List<ExpenseSummaryResponse>> GetExpenseSummaryAsync(ExpenseFilterRequest filter, Guid userId)
        {
            var expenses = new List<ExpenseSummaryResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("ExpenseSummary_Get", conn)
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
                    expenses.Add(new ExpenseSummaryResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        ExpenseDate = reader.GetDateTime(reader.GetOrdinal("ExpenseDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Cash = reader.IsDBNull(reader.GetOrdinal("Cash")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Cash")),
                        SBIAccount = reader.IsDBNull(reader.GetOrdinal("SBIAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SBIAccount")),
                        CBIAccount = reader.IsDBNull(reader.GetOrdinal("CBIAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CBIAccount")),
                        Other = reader.IsDBNull(reader.GetOrdinal("Other")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Other")),
                        CashBalance = reader.IsDBNull(reader.GetOrdinal("CashBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CashBalance")),
                        SBIBalance = reader.IsDBNull(reader.GetOrdinal("SBIBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SBIBalance")),
                        CBIBalance = reader.IsDBNull(reader.GetOrdinal("CBIBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CBIBalance")),
                        OtherBalance = reader.IsDBNull(reader.GetOrdinal("OtherBalance")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OtherBalance")),
                        TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                        TotalAvailable = reader.IsDBNull(reader.GetOrdinal("TotalAvailable")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAvailable"))

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
                throw new Exception("An error occurred while retrieving expenses.", ex);
            }

            return expenses;
        }

        public async Task<List<ExpenseReportResponse>> GetExpenseReportAsync(ExpenseFilterRequest filter, Guid userId)
        {
            var expenses = new List<ExpenseReportResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("ExpenseReport_Get", conn)
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
                    expenses.Add(new ExpenseReportResponse
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
                throw new Exception("An error occurred while retrieving expenses.", ex);
            }

            return expenses;
        }

        public async Task<ExpenseDetailsResponse> GetExpenseDetailsAsync(Guid expenseId, Guid userId)
        {
            var result = new ExpenseDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_ExpenseId", SqlDbType.UniqueIdentifier) { Value = expenseId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new ExpenseDetailsResponse
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? null : reader.GetGuid(reader.GetOrdinal("Id")),
                        ExpenseDate = reader.IsDBNull(reader.GetOrdinal("ExpenseDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ExpenseDate")),
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Cash = reader.IsDBNull(reader.GetOrdinal("Cash")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Cash")),
                        SbiAccount = reader.IsDBNull(reader.GetOrdinal("SbiAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SbiAccount")),
                        CbiAccount = reader.IsDBNull(reader.GetOrdinal("CbiAccount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("CbiAccount")),
                        Other = reader.IsDBNull(reader.GetOrdinal("Other")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Other")),
                        AssetId = reader.IsDBNull(reader.GetOrdinal("AssetId")) ? null : reader.GetGuid(reader.GetOrdinal("AssetId")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving expense details.", ex);
            }

            return result;
        }

        public async Task<ExpenseSuggestionList> GetExpenseSuggestionListAsync(Guid userId)
        {
            var result = new ExpenseSuggestionList();
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
                    result = new ExpenseSuggestionList
                    {
                        SourceOrReason = reader.IsDBNull(reader.GetOrdinal("SourceOrReason")) ? null : reader.GetString(reader.GetOrdinal("SourceOrReason")),
                        Purpose = reader.IsDBNull(reader.GetOrdinal("Purpose")) ? null : reader.GetString(reader.GetOrdinal("Purpose")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving expense details.", ex);
            }

            return result;
        }

        public async Task<Guid>? AddAsync(ExpenseRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add Input Parameters (matching your class)
                cmd.Parameters.AddWithValue("@in_SourceOrReason", request.SourceOrReason);
                cmd.Parameters.AddWithValue("@in_ExpenseDate", request.ExpenseDate);
                cmd.Parameters.AddWithValue("@in_Purpose", request.Purpose);
                cmd.Parameters.AddWithValue("@in_Cash", request.Cash);
                cmd.Parameters.AddWithValue("@in_SbiAccount", request.SbiAccount);
                cmd.Parameters.AddWithValue("@in_CbiAccount", request.CbiAccount);
                cmd.Parameters.AddWithValue("@in_Other", request.Other);
                cmd.Parameters.AddWithValue("@in_Description", request.Description);
                cmd.Parameters.AddWithValue("@in_UserId", userId);

                // Add Output Parameter
                var outIdParam = new SqlParameter("@out_ReportStatus", SqlDbType.UniqueIdentifier)
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

        public async Task<bool> UpdateAsync(ExpenseRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_ExpenseId", request.Id);
                cmd.Parameters.AddWithValue("@in_ExpenseDate", request.ExpenseDate);
                cmd.Parameters.AddWithValue("@in_SourceOrReason", request.SourceOrReason);
                cmd.Parameters.AddWithValue("@in_Purpose", request.Purpose);
                cmd.Parameters.AddWithValue("@in_Cash", request.Cash);
                cmd.Parameters.AddWithValue("@in_SbiAccount", request.SbiAccount);
                cmd.Parameters.AddWithValue("@in_CbiAccount", request.CbiAccount);
                cmd.Parameters.AddWithValue("@in_Other", request.Other);
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

        public async Task<bool> DeleteAsync(Guid expenseId, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Expense_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_ExpenseId", expenseId);
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
