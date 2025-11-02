
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
        private readonly AzureBlobService _blobService;

        public SpecialOccasionService(ISpecialOccasionRepository repository, AzureBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }

        public async Task<ServiceResponse<List<SpecialOccasion>>> GetAllDaysAsync()
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
            //foreach (var item in response.Data)
            //{
            //    if (!string.IsNullOrEmpty(item.ImagePath))
            //    {
            //        item.ImagePath = _blobService.GetBlobSasUrl(item.ImagePath);
            //    }

            //    if (!string.IsNullOrEmpty(item.ThumbnailPath))
            //    {
            //        item.ThumbnailPath = _blobService.GetBlobSasUrl(item.ThumbnailPath);
            //    }
            //}

            return response;
           
        }

        public async Task<ServiceResponse<SpecialOccasionDetailsResponse>> GetDayDetailsAsync(Guid dayId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetDayDetailsAsync(dayId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayDetailsFetchedSuccessfully
           );          
        }

        public async Task<ServiceResponse<Guid>> AddDayAsync(SpecialOccasionRequest Day, Guid userId)
        {   
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(Day, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayCreatedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> UpdateDayAsync(SpecialOccasionRequest Day, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(Day, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DayUpdatedSuccessfully
            );
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
