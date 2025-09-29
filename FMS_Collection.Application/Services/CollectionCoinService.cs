
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
            var response = new ServiceResponse<List<CoinNoteCollection>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }// => 
        public async Task<ServiceResponse<List<CoinNoteCollectionListResponse>>> GetCoinNoteCollectionListAsync(Guid userId)
        {
            var response = new ServiceResponse<List<CoinNoteCollectionListResponse>>();
            try
            {
                var data = await _repository.GetCoinNoteCollectionListAsync(userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionListFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<CoinNoteCollectionDetailsResponse>> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId)
        {
            var response = new ServiceResponse<CoinNoteCollectionDetailsResponse>();
            try
            {
                var data = await _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDetailsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<Guid>> AddCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection,Guid coinNoteCollectionId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddAsync(CoinNoteCollection, coinNoteCollectionId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> UpdateCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateAsync(CoinNoteCollection, coinNoteCollectionId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> DeleteCoinNoteCollectionAsync(Guid coinNoteCollectionId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(coinNoteCollectionId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionDeletedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<List<CoinNoteCollectionSummaryResponse>>> GetSummaryAsync()
        {
            var response = new ServiceResponse<List<CoinNoteCollectionSummaryResponse>>();
            try
            {
                var data = await _repository.GetSummaryAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.CoinNoteCollectionSummaryFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
    }
}
