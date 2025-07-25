
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface ICommonListRepository
    {
        Task<List<CommonList>> GetAllCommonListAsync();
        Task<List<CommonListItem>> GetAllCommonListItemAsync();
        
        Task<List<CommonListResponse>> GetCommonListAsync();
        Task<List<CommonListItemResponse>> GetCommonListItemAsync(Guid CommonListId);
        
        Task<CommonListResponse> GetCommonListDetailsAsync(Guid CommonListItemId);
        Task<CommonListItemResponse> GetCommonListItemDetailsAsync(Guid CommonListItemId);

        Task AddCommonListAsync(CommonListRequest CommonList, Guid CommonId);
        Task AddCommonListItemAsync(CommonListItemRequest CommonListItem, Guid CommonId);

        Task UpdateCommonListAsync(CommonListRequest Common, Guid CommonId);
        Task UpdateCommonListItemAsync(CommonListItemRequest Common, Guid CommonId);

        Task<bool> DeleteCommonListAsync(Guid CommonId, Guid userId);
        Task<bool> DeleteCommonListItemAsync(Guid CommonId, Guid userId);
        Task<List<CountryWithCurrency>> GetCountryListAsync();
    }
}
