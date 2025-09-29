
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class CommonListService
    {
        private readonly ICommonListRepository _repository;
        public CommonListService(ICommonListRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<CommonList>>> GetAllCommonListAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllCommonListAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListsFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<List<CommonListItem>>> GetAllCommonListItemAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllCommonListItemAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemsFetchedSuccessfully
            );
        }
        
        
        public async Task<ServiceResponse<List<CommonListResponse>>> GetCommonListAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCommonListAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListsFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<List<CommonListItemResponse>>> GetCommonListItemAsync(Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCommonListItemAsync(CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemsFetchedSuccessfully
            );
        }
        
        
        public async Task<ServiceResponse<CommonListResponse>> GetCommonListDetailsAsync(Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCommonListDetailsAsync(CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListDetailsFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<CommonListItemResponse>> GetCommonListItemDetailsAsync(Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCommonListItemDetailsAsync(CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemDetailsFetchedSuccessfully
            );
        }
        
        
        public async Task<ServiceResponse<Guid>> AddCommonListAsync(CommonListRequest Common,Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddCommonListAsync(Common, CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListCreatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<Guid>> AddCommonListItemAsync(CommonListItemRequest Common,Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddCommonListItemAsync(Common, CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemCreatedSuccessfully
            );
        }
        
        
        public async Task<ServiceResponse<bool>> UpdateCommonListAsync(CommonListRequest Common, Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateCommonListAsync(Common, CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListUpdatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> UpdateCommonListItemAsync(CommonListItemRequest Common, Guid CommonId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateCommonListItemAsync(Common, CommonId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemUpdatedSuccessfully
            );
        }
        
        
        public async Task<ServiceResponse<bool>> DeleteCommonListAsync(Guid CommonId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteCommonListAsync(CommonId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListDeletedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> DeleteCommonListItemAsync(Guid CommonId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteCommonListItemAsync(CommonId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CommonListItemDeletedSuccessfully
            );
        }
        

        public async Task<ServiceResponse<List<CountryWithCurrency>>> GetCountryListAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCountryListAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CountriesFetchedSuccessfully
            );
        }
        
    }
}
