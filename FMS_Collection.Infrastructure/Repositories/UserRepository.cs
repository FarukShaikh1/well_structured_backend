using Azure.Core;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public UserRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("User_GetAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                        EmailAddr = reader.IsDBNull(reader.GetOrdinal("EmailAddr")) ? null : reader.GetString(reader.GetOrdinal("EmailAddr")),
                        UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        PasswordLastChangedOn = reader.IsDBNull(reader.GetOrdinal("PasswordLastChangedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PasswordLastChangedOn")),
                        EffectiveDt = reader.IsDBNull(reader.GetOrdinal("EffectiveDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EffectiveDt")),
                        ExpirationDt = reader.IsDBNull(reader.GetOrdinal("ExpirationDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpirationDt")),
                        ClientGroupCountry = reader.IsDBNull(reader.GetOrdinal("ClientGroupCountry")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("ClientGroupCountry")),
                        FailedLoginCount = reader.IsDBNull(reader.GetOrdinal("FailedLoginCount")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("FailedLoginCount")),
                        LockExpiryDate = reader.IsDBNull(reader.GetOrdinal("LockExpiryDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LockExpiryDate")),
                        SpecialOccasionId = reader.IsDBNull(reader.GetOrdinal("SpecialOccasionId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("SpecialOccasionId")),
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
                throw new Exception("An error occurred while retrieving users.", ex);
            }

            return users;
        }

        public async Task<List<UserListResponse>> GetUserListAsync(Guid userId)
        {
            var users = new List<UserListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("User_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;

                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new UserListResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        UserName = reader["UserName"] as string,
                        LastName = reader["LastName"] as string,
                        FirstName = reader["FirstName"] as string,
                        Password = reader["Password"] as string,
                        EmailAddress = reader["EmailAddress"] as string,
                        //MobileNumber = reader["MobileNumber"] as string,
                        IsLocked = reader["IsLocked"] != DBNull.Value ? (bool?)reader["IsLocked"] : null,
                        RoleName = reader["RoleName"] as string
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }

            return users;
        }

        public async Task<UserDetailsResponse> GetUserDetailsAsync(Guid userId)
        {
            var result = new UserDetailsResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("User_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new UserDetailsResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null,
                        LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null,
                        UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null,
                        EmailAddress = reader["EmailAddress"] != DBNull.Value ? reader["EmailAddress"].ToString() : null,
                        Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : null,
                        PasswordLastChangeOn = reader["PasswordLastChangedOn"] != DBNull.Value ? (DateTime?)reader["PasswordLastChangedOn"] : null,
                        RoleId = reader["RoleId"] != DBNull.Value ? (Guid?)reader["RoleId"] : null,
                        specialOccasionDate = reader["specialOccasionDate"] != DBNull.Value ? (DateTime?)reader["specialOccasionDate"] : null,
                        MobileNumber = reader["MobileNumber"] != DBNull.Value ? reader["MobileNumber"].ToString() : null,
                        AlternateNumber = reader["AlternateNumber"] != DBNull.Value ? reader["AlternateNumber"].ToString() : null,
                        Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                        IsDeleted = reader["IsDeleted"] != DBNull.Value ? (bool?)reader["IsDeleted"] : null
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user details.", ex);
            }

            return result;
        }

        public async Task<Guid> AddAsync(UserRequest request, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("SpecialOccasion_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add Input Parameters (matching your class)
                cmd.Parameters.AddWithValue("@In_RelationId", request.Password);
                cmd.Parameters.AddWithValue("@in_UserId", userId);

                // Add Output Parameter
                var outIdParam = new SqlParameter("@Out_SpecialOccasionId", SqlDbType.UniqueIdentifier)
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

        public async Task UpdateAsync(UserRequest request   , Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("User_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("User_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
            return true;
        }

        private void AddUserRequestParameters(SqlCommand cmd, UserRequest user, Guid userId)
        {

        }

        public async Task<UserPermissionResponse> GetUserPermission(Guid userId)
        {
            var result = new UserPermissionResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("UserPermission_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userId });

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new UserPermissionResponse
                    {
                        UserPermissionId = reader["UserPermissionId"] != DBNull.Value ? (Guid?)reader["UserPermissionId"] : null,
                        ModuleId = reader["ModuleId"] != DBNull.Value ? (Guid?)reader["ModuleId"] : null,
                        ModuleName = reader["ModuleName"] != DBNull.Value ? reader["ModuleName"].ToString() : null,
                        View = reader["View"] != DBNull.Value ? (bool?)reader["View"] : null,
                        Add = reader["Add"] != DBNull.Value ? (bool?)reader["Add"] : null,
                        Edit = reader["Edit"] != DBNull.Value ? (bool?)reader["Edit"] : null,
                        Delete = reader["Delete"] != DBNull.Value ? (bool?)reader["Delete"] : null,
                        Download = reader["Download"] != DBNull.Value ? (bool?)reader["Download"] : null,
                        Upload = reader["Upload"] != DBNull.Value ? (bool?)reader["Upload"] : null,
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
                throw new Exception("An error occurred while retrieving user details.", ex);
            }

            return result;
        }

        public async Task<bool> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("UserPermission_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = userPermission.UserId });
                cmd.Parameters.Add(new SqlParameter("@in_ModuleId", SqlDbType.UniqueIdentifier) { Value = userPermission.ModuleId });
                cmd.Parameters.Add(new SqlParameter("@in_View", SqlDbType.Bit) { Value = userPermission.View ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_Add", SqlDbType.Bit) { Value = userPermission.Add ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_Edit", SqlDbType.Bit) { Value = userPermission.Edit ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_Delete", SqlDbType.Bit) { Value = userPermission.Delete ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_Download", SqlDbType.Bit) { Value = userPermission.Download ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_Upload", SqlDbType.Bit) { Value = userPermission.Upload ?? false });
                cmd.Parameters.Add(new SqlParameter("@in_ModifiedBy", SqlDbType.UniqueIdentifier) { Value = userId });
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
            return true;
        }

        public async Task<LoginResponse> GetLoginDetails(LoginRequest user)
        {
            var result = new LoginResponse();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("UserLogin_Details_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@in_UserName", SqlDbType.VarChar) { Value = user.UserName});
                cmd.Parameters.Add(new SqlParameter("@in_Password", SqlDbType.VarChar) { Value = user.Password});

                conn.Open();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new LoginResponse
                    {
                        Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
                        FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null,
                        LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null,
                        UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null,
                        EmailAddress = reader["EmailAddress"] != DBNull.Value ? reader["EmailAddress"].ToString() : null,
                        Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : null,
                        PasswordLastChangeDate = reader["PasswordLastChangeDate"] != DBNull.Value ? (DateTime?)reader["PasswordLastChangeDate"] : null,
                        RoleId = reader["RoleId"] != DBNull.Value ? (Guid?)reader["RoleId"] : null,
                        RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : null,
                        AccessibleModuleIds = new List<Guid>() // Will be populated below
                    };
                }
                // Move to second result set (AccessibleModuleIds)
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader["ModuleId"] != DBNull.Value)
                            result.AccessibleModuleIds.Add((Guid)reader["ModuleId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user details.", ex);
            }

            return result;
        }

        public async Task<List<ModuleListResponse>> GetModuleListAsync()
        {
            var users = new List<ModuleListResponse>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                using var cmd = new SqlCommand("Module_Get", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 120;
                conn.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new ModuleListResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        ModuleName = reader["ModuleName"] as string,
                        Description = reader["Description"] as string,
                        IsActive = reader["IsActive"] as bool?,
                        Route = reader["Route"] as string,
                        DisplayOrder = reader["DisplayOrder"] as int?,
                        IconClass = reader["IconClass"] as string
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users.", ex);
            }

            return users;
        }

    }
}
