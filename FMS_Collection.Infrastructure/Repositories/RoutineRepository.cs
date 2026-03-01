using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public RoutineRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private Routine MapRoutine(SqlDataReader reader)
        {
            return new Routine
            {
                Id = reader.GetValue<Guid?>("Id"),
                UserId = reader.GetValue<Guid>("UserId"),
                FromTime = reader.GetValue<TimeSpan>("FromTime"),
                ToTime = reader.GetValue<TimeSpan>("ToTime"),
                Task = reader.GetValue<string>("Task"),
                CreatedOn = reader.GetValue<DateTime?>("CreatedOn"),
                CreatedBy = reader.GetValue<Guid?>("CreatedBy"),
                ModifiedOn = reader.GetValue<DateTime?>("ModifiedOn"),
                ModifiedBy = reader.GetValue<Guid?>("ModifiedBy")
            };
        }

        public async Task<List<Routine>> GetAllAsync()
        {
            var routines = new List<Routine>();

            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Routine_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 120
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    routines.Add(MapRoutine(reader));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return routines;
        }

        public async Task<List<Routine>> GetRoutineListAsync(Guid userId)
        {
            var routines = new List<Routine>();

            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Routine_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 120
                };

                cmd.Parameters.Add("@in_UserId", SqlDbType.UniqueIdentifier).Value = userId;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    routines.Add(MapRoutine(reader));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return routines;
        }

        public async Task<Routine?> GetRoutineDetailsAsync(Guid routineId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Routine_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@in_RoutineId", SqlDbType.UniqueIdentifier).Value = routineId;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapRoutine(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return null;
        }

        public async Task<Guid> AddAsync(Routine routine, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Routine_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                RoutineParameters(cmd, routine, userId);

                var outId = new SqlParameter("@out_Id", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return outId.Value != DBNull.Value ? (Guid)outId.Value : Guid.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<bool> UpdateAsync(Routine routine, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Routine_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@in_RoutineId", SqlDbType.UniqueIdentifier).Value = routine.Id;
                RoutineParameters(cmd, routine, userId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        // -------------------- DELETE (SOFT) --------------------
        public async Task<bool> DeleteAsync(Guid routineId, Guid userId)
        {
            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand("Routine_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_RoutineId", routineId);
            cmd.Parameters.AddWithValue("@in_UserId", userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return true;
        }
        private void RoutineParameters(SqlCommand cmd, Routine routine, Guid userId)
        {
            cmd.Parameters.Add("@in_UserId", SqlDbType.UniqueIdentifier).Value = routine.UserId;
            cmd.Parameters.Add("@in_FromTime", SqlDbType.Time).Value = routine.FromTime;
            cmd.Parameters.Add("@in_ToTime", SqlDbType.Time).Value = routine.ToTime;
            cmd.Parameters.Add("@in_Task", SqlDbType.NVarChar, 200).Value = routine.Task ?? string.Empty;

            cmd.Parameters.Add("@in_ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
        }
    }
}
