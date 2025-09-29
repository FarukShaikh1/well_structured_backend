
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class SpecialOccasionService
    {
        private readonly ISpecialOccasionRepository _repository;
        public SpecialOccasionService(ISpecialOccasionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<SpecialOccasion>>> GetAllDaysAsync()
        {
            var response = new ServiceResponse<List<SpecialOccasion>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DaysFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<SpecialOccasionListResponse>>> GetDayListAsync(Guid userId)
        {
            var response = new ServiceResponse<List<SpecialOccasionListResponse>>();
            try
            {
                var data = await _repository.GetDayListAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DayListFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<SpecialOccasionDetailsResponse>> GetDayDetailsAsync(Guid dayId, Guid userId)
        {
            var response = new ServiceResponse<SpecialOccasionDetailsResponse>();
            try
            {
                var data = await _repository.GetDayDetailsAsync(dayId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DayDetailsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> AddDayAsync(SpecialOccasionRequest Day, Guid userId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddAsync(Day, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DayCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateDayAsync(SpecialOccasionRequest Day, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateAsync(Day, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DayUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteDayAsync(Guid dayId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(dayId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.DayDeletedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
