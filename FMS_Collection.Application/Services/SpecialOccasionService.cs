
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class SpecialOccasionService
    {
        private readonly ISpecialOccasionRepository _repository;
        private readonly AzureBlobService _blobService;
        private readonly UserService _userService;

        public SpecialOccasionService(ISpecialOccasionRepository repository, AzureBlobService blobService, UserService userService)
        {
            _repository = repository;
            _blobService = blobService;
            _userService = userService;
        }

        public async Task<ServiceResponse<List<SpecialOccasions>>> GetAllDaysAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.DaysFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<List<SpecialOccasionListResponse>>> GetDayListAsync(Guid userId)
        {
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.GetDayListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayListFetchedSuccessfully
            );

            // Null or empty check
            if (response?.Data == null || !response.Data.Any())
                return response;

            //// Replace ImagePath and ThumbnailPath with Blob SAS URLs
            foreach (var item in response.Data)
            {
                if (!string.IsNullOrEmpty(item.ThumbnailPath))
                {
                    item.ThumbnailPathSasUrl = _blobService.GetBlobSasUrl(item.ThumbnailPath);
                }
            }
            return response;
        }

        public async Task<ServiceResponse<SpecialOccasionDetailsResponse>> GetDayDetailsAsync(Guid dayId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetDayDetailsAsync(dayId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayDetailsFetchedSuccessfully
           );
        }

        public async Task<ServiceResponse<SpecialOccasionDetailsResponse>> AddDayAsync(SpecialOccasionRequest Day, Guid userId)
        {
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(Day, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayCreatedSuccessfully
            );
            if (!String.IsNullOrEmpty(response.Data.EmailId))
            {
                var loggedInUserData = await _userService.GetUserDetailsAsync(userId);
                if (loggedInUserData != null && loggedInUserData.Data != null && (loggedInUserData.Data.RoleName == Constants.Roles.SuperAdmin || loggedInUserData.Data.RoleName == Constants.Roles.Admin))
                {
                    var userResult = await _userService.GetUserDetailsAsync(null, response.Data.EmailId);
                    if (response != null && response.Success && response.Data != null
                        && !string.IsNullOrEmpty(response.Data.EmailId) && response.Data.DayTypeName.Equals("Birthday") && string.IsNullOrEmpty(userResult.Data.EmailAddress))//check its birthday from SP
                    {
                        UserRequest User = new UserRequest
                        {
                            SpecialOccasionId = response.Data.Id,
                            Password = RandomGeneratorService.GeneratePassword(10),
                            EmailAddress = Day.EmailId.ToString(),
                        };
                        User.Id = await _userService.AddUserAsync(User, userId);
                    }
                }
            }
            return response;
        }

        public async Task<ServiceResponse<SpecialOccasionDetailsResponse>> UpdateDayAsync(SpecialOccasionRequest Day, Guid userId)
        {
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(Day, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayUpdatedSuccessfully
            );
            if (!String.IsNullOrEmpty(response.Data.EmailId))
            {
                var loggedInUserData = await _userService.GetUserDetailsAsync(userId);
                if (loggedInUserData != null && loggedInUserData.Data != null && (loggedInUserData.Data.RoleName == Constants.Roles.SuperAdmin || loggedInUserData.Data.RoleName == Constants.Roles.Admin))
                {
                    var newUserResult = await _userService.GetUserDetailsAsync(null, response.Data.EmailId);
                    if (response != null && response.Success && response.Data != null && !string.IsNullOrEmpty(response.Data.EmailId) && (response?.Data?.DayTypeName).Equals("Birthday") && string.IsNullOrEmpty(newUserResult.Data.EmailAddress))//check its birthday from SP
                    {
                        UserRequest User = new UserRequest
                        {
                            SpecialOccasionId = response.Data.Id,
                            Password = RandomGeneratorService.GeneratePassword(10),
                            EmailAddress = Day.EmailId.ToString(),
                        };
                        User.Id = await _userService.AddUserAsync(User, userId);
                        if (User.Id != null)
                        {
                            response.Message += "User Added Successfully";
                        }
                    }
                }
                else
                {
                    response.Message = response.Message + ", LoggedInUserRoleName : " + loggedInUserData?.Data?.RoleName + ", ";
                }
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteDayAsync(Guid dayId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(dayId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayDeletedSuccessfully
            );
        }
    }
}
