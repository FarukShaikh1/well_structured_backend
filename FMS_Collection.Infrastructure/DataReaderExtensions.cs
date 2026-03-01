using Microsoft.Data.SqlClient;

public static class DataReaderExtensions
{
    public static T GetValue<T>(this SqlDataReader reader, string column)
    {
        return reader[column] == DBNull.Value ? default! : (T)reader[column];
    }
}
