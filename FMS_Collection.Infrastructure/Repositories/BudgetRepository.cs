using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public BudgetRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // -------------------- GET ALL --------------------
        public async Task<List<BudgetResponse>> GetAllAsync()
        {
            var result = new List<BudgetResponse>();

            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_GetAll", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapBudgetResponse(reader));
            }

            return result;
        }

        // -------------------- GET BY USER --------------------
        public async Task<List<BudgetResponse>> GetByUserAsync(Guid userId)
        {
            var result = new List<BudgetResponse>();

            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_GetByUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapBudgetResponse(reader));
            }

            return result;
        }

        // -------------------- GET DETAILS --------------------
        public async Task<BudgetResponse?> GetDetailsAsync(Guid budgetId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_Details_Get", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@in_BudgetId", budgetId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapBudgetResponse(reader);
            }

            return null;
        }

        // -------------------- ADD --------------------
        public async Task<Guid> AddAsync(Budget budget, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_Add", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            BudgetParameters(cmd, budget, userId);

            var outId = new SqlParameter("@out_BudgetId", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (Guid)outId.Value;
        }

        // -------------------- UPDATE --------------------
        public async Task<bool> UpdateAsync(Budget budget, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_BudgetId", budget.Id);
            BudgetParameters(cmd, budget, userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true;
        }

        // -------------------- DELETE (SOFT) --------------------
        public async Task<bool> DeleteAsync(Guid budgetId, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Budget_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_BudgetId", budgetId);
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true;
        }

        // -------------------- PARAMETERS --------------------
        private void BudgetParameters(SqlCommand cmd, Budget budget, Guid userId)
        {
            cmd.Parameters.AddWithValue("@in_UserId", userId);
            cmd.Parameters.AddWithValue("@in_PayTo", budget.PayTo);
            cmd.Parameters.AddWithValue("@in_Purpose", budget.Purpose);
            cmd.Parameters.AddWithValue("@in_CategoryId", budget.CategoryId);
            cmd.Parameters.AddWithValue("@in_Amount", budget.Amount);
        }

        // -------------------- MAPPER --------------------
        private static BudgetResponse MapBudgetResponse(SqlDataReader reader)
        {
            return new BudgetResponse
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                PayTo = reader.GetString(reader.GetOrdinal("PayTo")),
                Purpose = reader.GetString(reader.GetOrdinal("Purpose")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                ModifiedOn = reader.IsDBNull(reader.GetOrdinal("ModifiedOn"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("ModifiedOn"))
            };
        }
    }
}
