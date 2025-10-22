
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<List<UserListResponse>> GetUserListAsync(Guid userId);
        Task<UserDetailsResponse> GetUserDetailsAsync(Guid? userId, string? emailId=null);
        Task<Guid> AddAsync(UserRequest user, Guid userId);
        Task UpdateAsync(UserRequest user, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId);
        Task<LoginResponse> GetLoginDetails(LoginRequest user);
        Task<List<ModuleListResponse>> GetModuleListAsync();
        Task<List<UserPermissionResponse>> GetUserPermissionListAsync(Guid UserId);
        Task<bool> UpdatePasswordHashAsync(Guid? userId, string newPasswordHash);
    }
}
