
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
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<List<RoleListResponse>> GetRoleListAsync();
        Task<RoleDetailsResponse> GetRoleDetailsAsync(Guid roleId);
        Task AddAsync(RoleRequest role, Guid userId);
        Task UpdateAsync(RoleRequest role, Guid roleId);
        Task<bool> DeleteAsync(Guid roleId, Guid userId);
    }
}
