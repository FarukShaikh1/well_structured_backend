using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public SettingsRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }


        public async Task<List<ConfigurationResponse>> GetConfigListAsync(Guid userId, string config)
        {
            var Configs = new List<ConfigurationResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config});
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Configs.Add(new ConfigurationResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        ConfigurationName = reader["ConfigurationName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Configs;
        }

        public async Task<List<ConfigurationResponse>> GetActiveConfigListAsync(Guid userId, string config)
        {
            var Configs = new List<ConfigurationResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config });
                cmd.Parameters.Add(new SqlParameter("@in_ShowActiveOnly", SqlDbType.Bit) { Value = true });
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Configs.Add(new ConfigurationResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        ConfigurationName = reader["ConfigurationName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Configs;
        }

        public async Task<ConfigurationResponse> GetConfigDetailsAsync(Guid id, string config)
        {
            var result = new ConfigurationResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_ConfigId", SqlDbType.UniqueIdentifier) { Value = id });
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config });

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new ConfigurationResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        ConfigurationName = reader["ConfigurationName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? (DateTime?)reader["CreatedOn"] : null,
                        CreatedBy = reader["CreatedBy"] != DBNull.Value ? (Guid?)reader["CreatedBy"] : null,
                        ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)reader["ModifiedOn"] : null,
                        ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? (Guid?)reader["ModifiedBy"] : null,
                        IsDeleted = reader["IsDeleted"] != DBNull.Value ? (bool?)reader["IsDeleted"] : null
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return result;
        }

        public async Task<Guid>? AddConfigAsync(ConfigurationRequest request, Guid userId, string config)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add Input Parameters (matching your class)
                cmd.Parameters.AddWithValue("@in_ConfigName", request.ConfigurationName);
                cmd.Parameters.AddWithValue("@in_Description", request.Description);
                cmd.Parameters.AddWithValue("@in_DisplayOrder", request.DisplayOrder);
                cmd.Parameters.AddWithValue("@in_UserId", request.UserId);
                cmd.Parameters.AddWithValue("@in_CreatorId", userId);
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config });

                // Add Output Parameter
                var outIdParam = new SqlParameter("@out_ConfigId", SqlDbType.UniqueIdentifier)
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }
        }

        public async Task<bool> UpdateConfigAsync(ConfigurationRequest request, Guid userId, string config)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@in_Id", request.Id);
                cmd.Parameters.AddWithValue("@in_ConfigName", request.ConfigurationName);
                cmd.Parameters.AddWithValue("@in_Description", request.Description);
                cmd.Parameters.AddWithValue("@in_DisplayOrder", request.DisplayOrder);
                cmd.Parameters.AddWithValue("@in_UserId", userId);
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config});
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

        public async Task<bool> DeleteConfigAsync(Guid id, Guid userId, string config)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_Id", id);
                cmd.Parameters.AddWithValue("@in_UserId", userId);
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config });

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

        public async Task<bool> DeactivateConfigAsync(Guid id, Guid userId, string config)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Config_Deactivate", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@in_Id", id);
                cmd.Parameters.AddWithValue("@in_UserId", userId);
                cmd.Parameters.Add(new SqlParameter("@in_ConfigType", SqlDbType.VarChar) { Value = config });

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

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            var Accounts = new List<Account>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Account_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Accounts.Add(new Account
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        AccountName = reader["AccountName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Accounts;
        }

        public async Task<List<Relation>> GetAllRelationsAsync()
        {
            var Relations = new List<Relation>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Relation_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Relations.Add(new Relation
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        RelationName = reader["RelationName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return Relations;
        }

        public async Task<List<OccasionType>> GetAllOccasionTypesAsync()
        {
            var OccasionTypes = new List<OccasionType>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("OccasionType_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 600;
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    OccasionTypes.Add(new OccasionType
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        OccasionTypeName = reader["OccasionTypeName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int?)reader["DisplayOrder"] : null,
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool?)reader["IsActive"] : null,
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
                throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
            }

            return OccasionTypes;
        }
    }
}
