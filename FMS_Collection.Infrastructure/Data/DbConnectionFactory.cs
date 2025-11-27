// Infrastructure/Data/DbConnectionFactory.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace FMS_Collection.Infrastructure.Data;
public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("FMSConnectionString")!;
    }

    public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
}
