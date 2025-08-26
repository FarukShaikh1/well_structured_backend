
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

        public async Task<ServiceResponse<List<User>> >GetAllUsersAsync()
        {
            var response = new ServiceResponse<List<User>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<List<UserListResponse>>> GetUserListAsync(Guid userId)
        {
            var response = new ServiceResponse<List<UserListResponse>>();
            try
            {
                var data = await _repository.GetUserListAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<UserDetailsResponse>> GetUserDetailsAsync(Guid userId)
        {
            var response = new ServiceResponse<UserDetailsResponse>();
            try
            {
                var data = await _repository.GetUserDetailsAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task AddUserAsync(UserRequest User,Guid userId)
        {
            _repository.UpdateAsync(User, userId);
        }
        
        public async Task<ServiceResponse<Guid>> UpdateUserAsync(UserRequest User, Guid userId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddAsync(User, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> DeleteUserAsync(Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateUserPermissionAsync(userPermission, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        

        public async Task<ServiceResponse<LoginResponse>> GetLoginDetails(LoginRequest user)
        {
            var response = new ServiceResponse<LoginResponse>();
            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            user.Password = hashAlgorithm.GetHash(user.Password);
            try
            {
                var loginResponse = await _repository.GetLoginDetails(user);
                response.Success = true;
                response.Data = loginResponse;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            // Perform additional logic in UserService layer
            if (response.Data == null)
            {
                throw new Exception("Invalid user or password."); // Custom business logic
            }

            // Example: Mask the password before returning
            response.Data.Password = null;

            // Example: Log or audit
            // _logger.LogInformation($"User {response.Data.UserName} logged in at {DateTime.UtcNow}");

            // Example: Add a default module if user has no modules assigned
            if (response.Data.AccessibleModuleIds == null || !response.Data.AccessibleModuleIds.Any())
            {
                response.Data.AccessibleModuleIds = new List<Guid>
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111") // Default module id
        };
            }

            return response;
        }
        public async Task<ServiceResponse<List<ModuleListResponse>>> GetModuleListAsync()
        {
            var response = new ServiceResponse<List<ModuleListResponse>>();
            try
            {
                var data = await _repository.GetModuleListAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<List<UserPermissionResponse>>> GetUserPermissionListAsync(Guid UserId)
        {
            var response = new ServiceResponse<List<UserPermissionResponse>>();
            try
            {
                var data = await _repository.GetUserPermissionListAsync(UserId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
