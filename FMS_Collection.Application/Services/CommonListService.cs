
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Application.Services
{
    public class CommonListService
    {
        private readonly ICommonListRepository _repository;
        public CommonListService(ICommonListRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CommonList>> GetAllCommonListAsync() => _repository.GetAllCommonListAsync();
        public Task<List<CommonListItem>> GetAllCommonListItemAsync() => _repository.GetAllCommonListItemAsync();
        
        public Task<List<CommonListResponse>> GetCommonListAsync() => _repository.GetCommonListAsync();
        public Task<List<CommonListItemResponse>> GetCommonListItemAsync(Guid CommonId) => _repository.GetCommonListItemAsync(CommonId);
        
        public Task<CommonListResponse> GetCommonListDetailsAsync(Guid CommonId) => _repository.GetCommonListDetailsAsync(CommonId);
        public Task<CommonListItemResponse> GetCommonListItemDetailsAsync(Guid CommonId) => _repository.GetCommonListItemDetailsAsync(CommonId);
        
        public Task AddCommonListAsync(CommonListRequest Common,Guid CommonId) => _repository.AddCommonListAsync(Common, CommonId);
        public Task AddCommonListItemAsync(CommonListItemRequest Common,Guid CommonId) => _repository.AddCommonListItemAsync(Common, CommonId);
        
        public Task UpdateCommonListAsync(CommonListRequest Common, Guid CommonId) => _repository.UpdateCommonListAsync(Common, CommonId);
        public Task UpdateCommonListItemAsync(CommonListItemRequest Common, Guid CommonId) => _repository.UpdateCommonListItemAsync(Common, CommonId);
        
        public Task DeleteCommonListAsync(Guid CommonId, Guid userId) => _repository.DeleteCommonListAsync(CommonId, userId);
        public Task DeleteCommonListItemAsync(Guid CommonId, Guid userId) => _repository.DeleteCommonListItemAsync(CommonId, userId);

        public Task<List<CountryWithCurrency>> GetCountryListAsync() => _repository.GetCountryListAsync();
    }
}
