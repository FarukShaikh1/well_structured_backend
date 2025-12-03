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
        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          users.Add(new User
          {
            Id = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
            FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
            LastName = reader["LastName"]?.ToString(),
            EmailAddress = reader["EmailAddress"]?.ToString(),
            UserName = reader["UserName"]?.ToString(),
            Password = reader["Password"]?.ToString() ?? string.Empty,
            PasswordLastChangedOn = reader["PasswordLastChangedOn"] != DBNull.Value ? (DateTime?)reader["PasswordLastChangedOn"] : null,
            FailedLoginCount = reader["FailedLoginCount"] != DBNull.Value ? (int?)reader["FailedLoginCount"] : null,
            LockExpiryDate = reader["LockExpiryDate"] != DBNull.Value ? (DateTime?)reader["LockExpiryDate"] : null,
            CreatedOn = reader["CreatedOn"] != DBNull.Value ? (DateTime?)reader["CreatedOn"] : null,
            CreatedBy = reader["CreatedBy"] != DBNull.Value ? (Guid?)reader["CreatedBy"] : null,
            ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)reader["ModifiedOn"] : null,
            ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? (Guid?)reader["ModifiedBy"] : null,
            IsDeleted = reader["IsDeleted"] != DBNull.Value ? (bool?)reader["IsDeleted"] : null,
            RoleId = reader["RoleId"] != DBNull.Value ? (Guid?)reader["RoleId"] : null
          });
        }

      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
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

        await conn.OpenAsync();

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
            MobileNumber = reader["MobileNumber"] as string,
            IsLocked = reader["IsLocked"] != DBNull.Value ? (bool?)reader["IsLocked"] : null,
            RoleName = reader["RoleName"] as string,
            PasswordLastChangedOn = reader["PasswordLastChangedOn"] != DBNull.Value ? (DateTime?)reader["PasswordLastChangedOn"] : null,
          });
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }

      return users;
    }

    public async Task<UserDetailsResponse> GetUserDetailsAsync(Guid? userId, string? emailId)
    {
      var result = new UserDetailsResponse();
      try
      {
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand("User_Details_Get", conn)
        {
          CommandType = CommandType.StoredProcedure
        };
        // Both parameters exist in the SP now
        cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier)
        {
          Value = (object?)userId ?? DBNull.Value
        });

        cmd.Parameters.Add(new SqlParameter("@in_EmailId", SqlDbType.VarChar, 200)
        {
          Value = (object?)emailId ?? DBNull.Value
        });
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
          result = new UserDetailsResponse
          {
            Id = Guid.TryParse(reader["Id"]?.ToString(), out var idValue) ? idValue : null,
            FirstName = reader["FirstName"]?.ToString(),
            LastName = reader["LastName"]?.ToString(),
            UserName = reader["UserName"]?.ToString(),
            Password = reader["Password"]?.ToString(),
            EmailAddress = reader["EmailAddress"]?.ToString(),
            RoleId = Guid.TryParse(reader["RoleId"]?.ToString(), out var roleIdValue) ? roleIdValue : null,
            Birthdate = reader["Birthdate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["Birthdate"]): null,
            MobileNumber = reader["MobileNumber"]?.ToString(),
            FailedLoginCount = reader["FailedLoginCount"] != DBNull.Value? Convert.ToInt32(reader["FailedLoginCount"]): null,
            LockExpiryDate = reader["LockExpiryDate"] != DBNull.Value? (DateTime?)reader["LockExpiryDate"]: null,
            PasswordLastChangedOn = reader["PasswordLastChangedOn"] != DBNull.Value? (DateTime?)reader["PasswordLastChangedOn"]: null,
            ThumbnailPath = reader["ThumbnailPath"]?.ToString(),
            OriginalPath = reader["OriginalPath"]?.ToString(),
            Address = reader["Address"]?.ToString(),
            ModifiedBy = Guid.TryParse(reader["ModifiedBy"]?.ToString(), out var modByValue) ? modByValue : null,
            ModifiedOn = reader["ModifiedOn"] != DBNull.Value? (DateTime?)reader["ModifiedOn"]: null,
            IsLocked = reader["IsLocked"] != DBNull.Value ? (bool?)reader["IsLocked"] : null,
            IsDeleted = (bool)reader["IsDeleted"]
          };
        }

      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
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
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }
    }

    public async Task UpdateAsync(UserRequest request, Guid userId)
    {
      try
      {
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand("User_Update", conn)
        {
          CommandType = CommandType.StoredProcedure
        };

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
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

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }
      return true;
    }

    private void AddUserRequestParameters(SqlCommand cmd, UserRequest user, Guid userId)
    {

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
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
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
        cmd.Parameters.Add(new SqlParameter("@in_UserName", SqlDbType.VarChar) { Value = user.UserName });
        cmd.Parameters.Add(new SqlParameter("@in_Password", SqlDbType.VarChar) { Value = user.Password });

        await conn.OpenAsync();
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
            MobileNumber = reader["MobileNumber"] != DBNull.Value ? reader["MobileNumber"].ToString() : null,
            Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : null,
            PasswordLastChangeDate = reader["PasswordLastChangeDate"] != DBNull.Value ? (DateTime?)reader["PasswordLastChangeDate"] : null,
            FailedLoginCount = reader["FailedLoginCount"] != DBNull.Value ? (int?)reader["FailedLoginCount"] : null,
            LockExpiryDate = reader["LockExpiryDate"] != DBNull.Value ? (DateTime?)reader["LockExpiryDate"] : null,
            SpecialOccasionDate = reader["SpecialOccasionDate"] != DBNull.Value ? (DateTime?)reader["SpecialOccasionDate"] : null,
            IsOtpRequired = reader["IsOtpRequired"] != DBNull.Value && Convert.ToBoolean(reader["IsOtpRequired"]),
            IsDeleted = reader["IsDeleted"] != DBNull.Value && Convert.ToBoolean(reader["IsDeleted"]),
            RoleId = reader["RoleId"] != DBNull.Value ? (Guid?)reader["RoleId"] : null,
            RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : null
          };
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
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
        await conn.OpenAsync();

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
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }

      return users;
    }

    public async Task<List<UserPermissionResponse>> GetUserPermissionListAsync(Guid UserId)
    {
      var users = new List<UserPermissionResponse>();
      try
      {
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand("UserPermission_Get", conn)
        {
          CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@in_UserId", SqlDbType.UniqueIdentifier) { Value = UserId });
        cmd.CommandTimeout = 120;
        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          users.Add(new UserPermissionResponse
          {
            Id = reader["Id"] != DBNull.Value ? (Guid?)reader["Id"] : null,
            ModuleId = reader["ModuleId"] != DBNull.Value ? (Guid?)reader["ModuleId"] : null,
            ModuleName = reader["ModuleName"]?.ToString(),
            Route = reader["Route"]?.ToString(),
            Description = reader["Description"]?.ToString(),
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
            IsDeleted = (bool)reader["IsDeleted"]
          });
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }

      return users;
    }

    public async Task<bool> UpdatePasswordHashAsync(Guid? userId, string newPasswordHash)
    {
      try
      {
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand("Update_Password", conn)
        {
          CommandType = CommandType.StoredProcedure
        };

        // ✅ Input parameters
        cmd.Parameters.AddWithValue("@in_UserId", (object?)userId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@in_NewPassword", newPasswordHash ?? (object)DBNull.Value);

        // ✅ Output parameters
        var outIsSuccess = new SqlParameter("@out_IsSuccess", SqlDbType.Bit)
        {
          Direction = ParameterDirection.Output
        };

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        // ✅ Extract output parameter values
        bool isSuccess = outIsSuccess.Value != DBNull.Value && (bool)outIsSuccess.Value;

        return isSuccess;
      }
      catch (Exception ex)
      {
        throw new Exception(
            string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }
    }


    public async Task<(bool IsSuccess, string Message)> ChangePasswordHashAsync(string oldPasswordHash, string newPasswordHash, Guid? userId, Guid? modifiedBy)
    {
      try
      {
        using var conn = _dbFactory.CreateConnection();
        using var cmd = new SqlCommand("User_Change_Password", conn)
        {
          CommandType = CommandType.StoredProcedure
        };

        // ✅ Input parameters
        cmd.Parameters.AddWithValue("@in_UserId", (object?)userId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@in_OldPassword", oldPasswordHash ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@in_NewPassword", newPasswordHash ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@in_ModifiedBy", modifiedBy);

        // ✅ Output parameters
        var outIsSuccess = new SqlParameter("@out_IsSuccess", SqlDbType.Bit)
        {
          Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outIsSuccess);

        var outMessage = new SqlParameter("@out_Message", SqlDbType.NVarChar, 200)
        {
          Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outMessage);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        // ✅ Extract output parameter values
        bool isSuccess = outIsSuccess.Value != DBNull.Value && (bool)outIsSuccess.Value;
        string message = outMessage.Value?.ToString() ?? string.Empty;

        return (isSuccess, message);
      }
      catch (Exception ex)
      {
        throw new Exception(
            string.Format(FMS_Collection.Core.Constants.Constants.Messages.GenericErrorWithActual, ex), ex);
      }
    }
  }
}
