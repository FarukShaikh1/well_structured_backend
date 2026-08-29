using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using YourProject.Core.Entities;

public class EmailRepository : IEmailRepository
{
    private readonly DbConnectionFactory _dbFactory;

    public EmailRepository(DbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<EmailTemplate?> GetByCodeAsync(string templateCode, CancellationToken cancellationToken = default)
    {
        try
        {
            const string query = @"SELECT Id, TemplateCode, TemplateName, Subject, BodyHtml, IsActive, 
                CreatedDate, ModifiedDate 
                FROM EmailTemplates
                WHERE TemplateCode = @TemplateCode
                AND IsActive = 1;";

            using var conn = _dbFactory.CreateConnection();
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 120
            };
            cmd.Parameters.Add("@TemplateCode", SqlDbType.VarChar).Value = templateCode;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return MapEmailTemplate(reader);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<List<EmailTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT Id, TemplateCode, TemplateName, Subject, BodyHtml, IsActive, CreatedDate, ModifiedDate 
            FROM EmailTemplates
            ORDER BY TemplateName;";

        var templates = new List<EmailTemplate>();
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand(query, conn)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 120
        };
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync(cancellationToken))
        {
            templates.Add(MapEmailTemplate(reader));
        }

        return templates;
    }

    public async Task<EmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT Id, TemplateCode, TemplateName, Subject, BodyHtml, IsActive, CreatedDate, ModifiedDate
            FROM EmailTemplates WHERE Id = @Id;";

        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand(query, conn)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 120
        };
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return MapEmailTemplate(reader);
    }

    private static EmailTemplate MapEmailTemplate(SqlDataReader reader)
    {
        return new EmailTemplate
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),

            TemplateCode = reader.GetString(reader.GetOrdinal("TemplateCode")),

            TemplateName = reader.GetString(reader.GetOrdinal("TemplateName")),

            Subject = reader.GetString(reader.GetOrdinal("Subject")),

            BodyHtml = reader.GetString(reader.GetOrdinal("BodyHtml")),

            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),

            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

            ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
        };
    }
}