
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<List<UserListResponse>> GetUserListAsync(Guid userId);
        Task<UserDetailsResponse> GetUserDetailsAsync(Guid userId);
        Task<Guid> AddAsync(UserRequest user, Guid userId);
        Task UpdateAsync(UserRequest user, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<UserPermissionResponse> GetUserPermission(Guid userId);
        Task<bool> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId);
        Task<LoginResponse> GetLoginDetails(LoginRequest user);
        Task<List<ModuleListResponse>> GetModuleListAsync();
    }
}
