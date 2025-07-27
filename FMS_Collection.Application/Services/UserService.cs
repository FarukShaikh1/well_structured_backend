
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<List<User>> GetAllUsersAsync() => _repository.GetAllAsync();
        public Task<List<UserListResponse>> GetUserListAsync(Guid userId) => _repository.GetUserListAsync(userId);
        public Task<UserDetailsResponse> GetUserDetailsAsync(Guid userId) => _repository.GetUserDetailsAsync(userId);
        public Task AddUserAsync(UserRequest User,Guid userId) => _repository.UpdateAsync(User, userId);
        public Task UpdateUserAsync(UserRequest User, Guid userId) => _repository.AddAsync(User, userId);
        public Task DeleteUserAsync(Guid userId ) => _repository.DeleteAsync(userId);
        public Task<bool> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId) => _repository.UpdateUserPermissionAsync(userPermission, userId);

        public async Task<LoginResponse> GetLoginDetails(LoginRequest user)
        {
            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            user.Password = hashAlgorithm.GetHash(user.Password);
            var loginResponse = await _repository.GetLoginDetails(user);

            // Perform additional logic in UserService layer
            if (loginResponse == null)
            {
                throw new Exception("Invalid user or password."); // Custom business logic
            }

            // Example: Mask the password before returning
            loginResponse.Password = null;

            // Example: Log or audit
            // _logger.LogInformation($"User {loginResponse.UserName} logged in at {DateTime.UtcNow}");

            // Example: Add a default module if user has no modules assigned
            if (loginResponse.AccessibleModuleIds == null || !loginResponse.AccessibleModuleIds.Any())
            {
                loginResponse.AccessibleModuleIds = new List<Guid>
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111") // Default module id
        };
            }

            return loginResponse;
        }
        public Task<List<ModuleListResponse>> GetModuleListAsync() => _repository.GetModuleListAsync();
        public Task<List<UserPermissionResponse>> GetUserPermissionListAsync(Guid UserId) => _repository.GetUserPermissionListAsync(UserId);

    }
}
