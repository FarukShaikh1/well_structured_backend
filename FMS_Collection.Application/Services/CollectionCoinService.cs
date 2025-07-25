
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

        public Task<List<CoinNoteCollection>> GetAllCoinNoteCollectionsAsync() => _repository.GetAllAsync();
        public Task<List<CoinNoteCollectionListResponse>> GetCoinNoteCollectionListAsync(Guid userId) => _repository.GetCoinNoteCollectionListAsync(userId);
        public Task<CoinNoteCollectionDetailsResponse> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId) => _repository.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, userId);
        public Task<Guid> AddCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection,Guid coinNoteCollectionId) => _repository.AddAsync(CoinNoteCollection, coinNoteCollectionId);
        public Task<bool> UpdateCoinNoteCollectionAsync(CoinNoteCollectionRequest CoinNoteCollection, Guid coinNoteCollectionId) => _repository.UpdateAsync(CoinNoteCollection, coinNoteCollectionId);
        public Task<bool> DeleteCoinNoteCollectionAsync(Guid coinNoteCollectionId, Guid userId) => _repository.DeleteAsync(coinNoteCollectionId, userId);
    }
}
