using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class FoodMenuRepository : IFoodMenuRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public FoodMenuRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // -------------------- GET ALL --------------------

        public async Task<List<FoodMenuResponse>> GetAllAsync()
        {
            var result = new List<FoodMenuResponse>();

            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_GetAll", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapFoodMenuResponse(reader));
            }

            return result;
        }


        // -------------------- GET BY USER --------------------

        public async Task<List<FoodMenuResponse>> GetByUserAsync(Guid userId)
        {
            var result = new List<FoodMenuResponse>();

            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_GetByUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(MapFoodMenuResponse(reader));
            }

            return result;
        }


        // -------------------- GET DETAILS --------------------

        public async Task<FoodMenuResponse?> GetDetailsAsync(Guid foodMenuId)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_Details_Get", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_FoodMenuId", foodMenuId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapFoodMenuResponse(reader);
            }

            return null;
        }


        // -------------------- ADD --------------------

        public async Task<Guid> AddAsync(
            FoodMenu foodMenu,
            Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_Add", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            FoodMenuParameters(cmd, foodMenu, userId);

            var outId = new SqlParameter(
                "@out_FoodMenuId",
                SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return (Guid)outId.Value;
        }


        // -------------------- UPDATE --------------------

        public async Task<bool> UpdateAsync(
            FoodMenu foodMenu,
            Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_FoodMenuId",
                foodMenu.Id);

            FoodMenuParameters(cmd, foodMenu, userId);

            await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return true;
        }


        // -------------------- DELETE (SOFT) --------------------

        public async Task<bool> DeleteAsync(
            Guid foodMenuId,
            Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("FoodMenu_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_FoodMenuId",
                foodMenuId);

            cmd.Parameters.AddWithValue(
                "@in_UserId",
                userId);

            await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return true;
        }


        // -------------------- PARAMETERS --------------------

        private void FoodMenuParameters(
            SqlCommand cmd,
            FoodMenu foodMenu,
            Guid userId)
        {
            cmd.Parameters.AddWithValue(
                "@in_UserId",
                userId);

            cmd.Parameters.AddWithValue(
                "@in_MenuDate",
                foodMenu.MenuDate);

            cmd.Parameters.AddWithValue(
                "@in_Sequence",
                foodMenu.Sequence);

            cmd.Parameters.AddWithValue(
                "@in_Breakfast",
                (object?)foodMenu.Breakfast ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_Lunch",
                (object?)foodMenu.Lunch ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_EveningBreakfast",
                (object?)foodMenu.EveningBreakfast ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_Dinner",
                (object?)foodMenu.Dinner ?? DBNull.Value);
        }


        // -------------------- MAPPER --------------------

        private static FoodMenuResponse MapFoodMenuResponse(
            SqlDataReader reader)
        {
            return new FoodMenuResponse
            {
                Id = reader.GetGuid(
                    reader.GetOrdinal("Id")),

                UserId = reader.GetGuid(
                    reader.GetOrdinal("UserId")),

                MenuDate = reader.GetByte(
                    reader.GetOrdinal("MenuDate")),

                Sequence = reader.GetInt32(
                    reader.GetOrdinal("Sequence")),

                Breakfast = reader.IsDBNull(
                    reader.GetOrdinal("Breakfast"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Breakfast")),

                Lunch = reader.IsDBNull(
                    reader.GetOrdinal("Lunch"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Lunch")),

                EveningBreakfast = reader.IsDBNull(
                    reader.GetOrdinal("EveningBreakfast"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("EveningBreakfast")),

                Dinner = reader.IsDBNull(
                    reader.GetOrdinal("Dinner"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Dinner")),

                CreatedOn = reader.GetDateTime(
                    reader.GetOrdinal("CreatedOn")),

                ModifiedOn = reader.IsDBNull(
                    reader.GetOrdinal("ModifiedOn"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("ModifiedOn"))
            };
        }
    }
}