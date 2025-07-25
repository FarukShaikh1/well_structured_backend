using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Role>> GetAllRolesAsync() => _repository.GetAllAsync();
        public Task<List<RoleListResponse>> GetRoleListAsync() => _repository.GetRoleListAsync();
        public Task<RoleDetailsResponse> GetRoleDetailsAsync(Guid roleId) => _repository.GetRoleDetailsAsync(roleId);
        public Task AddRoleAsync(RoleRequest Role,Guid roleId) => _repository.AddAsync(Role, roleId);
        public Task UpdateRoleAsync(RoleRequest Role, Guid roleId) => _repository.UpdateAsync(Role, roleId);
        public Task DeleteRoleAsync(Guid roleId, Guid userId) => _repository.DeleteAsync(roleId, userId);
    }
}
