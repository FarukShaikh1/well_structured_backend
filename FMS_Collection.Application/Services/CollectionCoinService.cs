
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
        private readonly AzureBlobService _blobService;
        private readonly AssetService _assetService;
        public CoinNoteCollectionService(ICoinNoteCollectionRepository repository, AzureBlobService blobService, AssetService assetService)
        {
            _repository = repository;
            _blobService = blobService;
            _assetService = assetService;
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
            // Fetch data from repository
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionListFetchedSuccessfully
            );

            // Null or empty check
            if (response?.Data == null || !response.Data.Any())
                return response;

            // Replace ImagePath and ThumbnailPath with Blob SAS URLs
            foreach (var item in response.Data)
            {
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    item.ImagePath = _blobService.GetBlobSasUrl(item.ImagePath);
                }

                if (!string.IsNullOrEmpty(item.ThumbnailPath))
                {
                    item.ThumbnailPath = _blobService.GetBlobSasUrl(item.ThumbnailPath);
                }
            }

            return response;
        }

        public async Task<ServiceResponse<CoinNoteCollectionDetailsResponse>> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDetailsFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<Guid>> AddCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId)
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
            // first delete assets related to the selected coin/note
            CoinNoteCollectionDetailsResponse coinDetails = await _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId);
            var response = await _assetService.DeleteAssetAsync(coinDetails.AssetId, userId);
            if (response == null)
            {
                return await ServiceExecutor.ExecuteAsync(
                () => null,
                FMS_Collection.Core.Constants.Constants.Messages.IssueInCoinDeletionNoteCollection
            );
            }
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
