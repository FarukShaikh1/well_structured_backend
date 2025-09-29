
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class CoinNoteCollectionService
    {
        private readonly ICoinNoteCollectionRepository _repository;
        public CoinNoteCollectionService(ICoinNoteCollectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<CoinNoteCollection>>> GetAllCoinNoteCollectionsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionsFetchedSuccessfully
            );
        }// => 
        public async Task<ServiceResponse<List<CoinNoteCollectionListResponse>>> GetCoinNoteCollectionListAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionListFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<CoinNoteCollectionDetailsResponse>> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDetailsFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<Guid>> AddCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection,Guid coinNoteCollectionId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(CoinNoteCollection, coinNoteCollectionId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionCreatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> UpdateCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(CoinNoteCollection, coinNoteCollectionId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionUpdatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> DeleteCoinNoteCollectionAsync(Guid coinNoteCollectionId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(coinNoteCollectionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDeletedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<List<CoinNoteCollectionSummaryResponse>>> GetSummaryAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetSummaryAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionSummaryFetchedSuccessfully
            );
        }
        
    }
}
