
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System.Reflection.Metadata;

namespace FMS_Collection.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly OtpService _otpService;
        public UserService(IUserRepository repository, OtpService otpService)
        {
            _repository = repository;
            _otpService = otpService;
        }

        public async Task<ServiceResponse<List<User>>> GetAllUsersAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.UsersFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<List<UserListResponse>>> GetUserListAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetUserListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.UserListFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<UserDetailsResponse>> GetUserDetailsAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetUserDetailsAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.UserDetailsFetchedSuccessfully
            );
        }

        public async Task AddUserAsync(UserRequest User, Guid userId)
        {
            _repository.UpdateAsync(User, userId);
        }

        public async Task<ServiceResponse<Guid>> UpdateUserAsync(UserRequest User, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(User, userId),
                FMS_Collection.Core.Constants.Constants.Messages.UserSavedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.UserDeletedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateUserPermissionAsync(userPermission, userId),
                FMS_Collection.Core.Constants.Constants.Messages.UserPermissionsUpdatedSuccessfully
            );
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
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.LoginSuccessful;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            // Perform additional logic in UserService layer
            if (response.Data.EmailAddress == null || response.Data.Password != user.Password)
            {
                response.Success = true;
                response.Data = null;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.InvalidUserOrPassword;
                return response;
            }
            response.Data.Password = null;
            if (response.Data.FailedLoginCount > AppSettings.AllowedFailedLoginCount)
            {
                response.Success = true;
                response.Data = null;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.TooManyLoginAttempts;
                return response;
            }

            if (response.Data.IsOtpRequired)
            {
                SendOtpRequest request = new SendOtpRequest();
                request.EmailId = response.Data.EmailAddress;
                request.Purpose = Constants.OtpPurpose.Login;
                await _otpService.SendAsync(request);
            }
            return response;
        }
        public async Task<ServiceResponse<List<ModuleListResponse>>> GetModuleListAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetModuleListAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.ModulesFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<List<UserPermissionResponse>>> GetUserPermissionListAsync(Guid UserId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetUserPermissionListAsync(UserId),
                FMS_Collection.Core.Constants.Constants.Messages.UserPermissionsFetchedSuccessfully
            );
        }
    }
}
