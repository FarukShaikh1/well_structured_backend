using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public ErrorRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task AddErrorLog(Exception ex,
        string? parameters = null,
        Guid? loggedBy = null)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var command = new SqlCommand("LogError_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                command.Parameters.AddWithValue("@ErrorNumber", ex.HResult);
                command.Parameters.AddWithValue("@ErrorSeverity", ex is SqlException sqlEx ? sqlEx.Class : 0);
                command.Parameters.AddWithValue("@ErrorState", ex is SqlException sqlEx2 ? sqlEx2.State : 0);
                command.Parameters.AddWithValue("@ErrorLine", ex is SqlException sqlEx3 ? sqlEx3.LineNumber : 0);
                command.Parameters.AddWithValue("@ErrorProcedure",
                    ex is SqlException sqlEx4 ? sqlEx4.Procedure ?? string.Empty : string.Empty);

                command.Parameters.AddWithValue("@Parameters",
                    (object?)parameters ?? DBNull.Value);

                command.Parameters.AddWithValue("@LoggedBy",
                    (object?)loggedBy ?? DBNull.Value);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex1)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex1), ex1);
            }
        }


    }
}


