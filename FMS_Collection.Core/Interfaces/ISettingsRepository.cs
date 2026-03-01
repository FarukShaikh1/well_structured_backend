using FMS_Collection.Core.Entities.ConfigEntities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface ISettingsRepository
    {
        Task<List<ConfigurationResponse>> GetConfigListAsync(Guid userId, string config);
        Task<List<ConfigurationResponse>> GetActiveConfigListAsync(Guid userId, string config);
        Task<ConfigurationResponse> GetConfigDetailsAsync(Guid id, string config);
        Task<Guid> AddConfigAsync(ConfigurationRequest Config, Guid userId, string config);
        Task<bool> UpdateConfigAsync(ConfigurationRequest Config, Guid userId, string config);
        Task<bool> DeleteConfigAsync(Guid ConfigId, Guid userId, string config);
        Task<bool> DeactivateConfigAsync(Guid ConfigId, Guid userId, string config);

        Task<List<Accounts>> GetAllAccountsAsync();
        Task<List<Relations>> GetAllRelationsAsync();
        Task<List<OccasionTypes>> GetAllOccasionTypesAsync();
        Task<List<TransactionSubCategories>> GetAllTransactionSubCategoriesAsync();


    }
}
