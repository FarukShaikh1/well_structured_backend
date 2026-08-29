// Infrastructure/Repositories/FamilyRepository.cs
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Requests;
using FMS_Collection.Core.Responses;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class FamilyRepository(DbConnectionFactory dbFactory) : IFamilyRepository
    {
        // ── Search ────────────────────────────────────────────────────────────

        public async Task<List<FamilyPersonSearchResult>> SearchPersonsAsync(string name)
        {
            var results = new List<FamilyPersonSearchResult>();

            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyPerson_Search", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            cmd.Parameters.AddWithValue("@Name", name);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(MapSearchResult(reader));
            }

            return results;
        }

        // ── Graph ─────────────────────────────────────────────────────────────

        public async Task<FamilyGraphResponse> GetGraphAsync(Guid personId, int maxDepth = 5)
        {
            var graph = new FamilyGraphResponse { RootPersonId = personId.ToString() };

            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyPerson_GetGraph", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 60
            };
            cmd.Parameters.AddWithValue("@RootPersonId", personId);
            cmd.Parameters.AddWithValue("@MaxDepth",     maxDepth);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            // Result set 1 — nodes
            while (await reader.ReadAsync())
            {
                graph.Nodes.Add(new FamilyGraphNode
                {
                    Id               = reader["PersonId"].ToString()!,
                    Label            = reader["Label"]?.ToString() ?? string.Empty,
                    FirstName        = reader["FirstName"]?.ToString() ?? string.Empty,
                    LastName         = reader["LastName"]?.ToString()  ?? string.Empty,
                    DateOfBirth      = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                    Gender           = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString()![0] : null,
                    ProfileImagePath = reader["ProfileImagePath"]?.ToString(),
                    Depth            = reader["Depth"] != DBNull.Value ? (int)reader["Depth"] : 0,
                    IsRoot           = reader["IsRoot"] != DBNull.Value && (bool)reader["IsRoot"]
                });
            }

            // Result set 2 — edges
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    graph.Edges.Add(new FamilyGraphEdge
                    {
                        Id    = reader["RelationshipId"].ToString()!,
                        From  = reader["FromId"].ToString()!,
                        To    = reader["ToId"].ToString()!,
                        Label = reader["Label"]?.ToString() ?? string.Empty
                    });
                }
            }

            return graph;
        }

        // ── CRUD ──────────────────────────────────────────────────────────────

        public async Task<Guid> AddPersonAsync(FamilyPersonRequest request, Guid createdBy)
        {
            var newId = Guid.NewGuid();

            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyPerson_Add", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            cmd.Parameters.AddWithValue("@FirstName",       request.FirstName);
            cmd.Parameters.AddWithValue("@LastName",        request.LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth",     (object?)request.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender",          (object?)request.Gender?.ToString() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes",           (object?)request.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProfileImagePath",(object?)request.ProfileImagePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LinkedUserId",    (object?)request.LinkedUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy",       createdBy);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return newId;
        }

        public async Task UpdatePersonAsync(FamilyPersonRequest request, Guid updatedBy)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyPerson_Update", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            cmd.Parameters.AddWithValue("@PersonId",        request.PersonId!.Value);
            cmd.Parameters.AddWithValue("@FirstName",       request.FirstName);
            cmd.Parameters.AddWithValue("@LastName",        request.LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth",     (object?)request.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Gender",          (object?)request.Gender?.ToString() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes",           (object?)request.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProfileImagePath",(object?)request.ProfileImagePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy",       updatedBy);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddRelationshipAsync(FamilyRelationshipRequest request, Guid createdBy)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyRelationship_Add", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            cmd.Parameters.AddWithValue("@PersonId",        request.PersonId);
            cmd.Parameters.AddWithValue("@RelatedPersonId", request.RelatedPersonId);
            cmd.Parameters.AddWithValue("@RelationshipType",request.RelationshipType);
            cmd.Parameters.AddWithValue("@CreatedBy",       createdBy);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteRelationshipAsync(Guid relationshipId)
        {
            using var conn = dbFactory.CreateConnection();
            using var cmd  = new SqlCommand("FamilyRelationship_Delete", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
            cmd.Parameters.AddWithValue("@RelationshipId", relationshipId);
            cmd.Parameters.AddWithValue("@UpdatedBy",      Guid.Empty);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // ── Private Helpers ───────────────────────────────────────────────────

        private static FamilyPersonSearchResult MapSearchResult(SqlDataReader r) => new()
        {
            PersonId         = r["PersonId"].ToString()!,
            FullName         = r["FullName"]?.ToString()    ?? string.Empty,
            FirstName        = r["FirstName"]?.ToString()   ?? string.Empty,
            LastName         = r["LastName"]?.ToString()    ?? string.Empty,
            DateOfBirth      = r["DateOfBirth"] != DBNull.Value ? (DateTime?)r["DateOfBirth"] : null,
            Gender           = r["Gender"] != DBNull.Value ? r["Gender"].ToString()![0] : null,
            ProfileImagePath = r["ProfileImagePath"]?.ToString()
        };
    }
}
