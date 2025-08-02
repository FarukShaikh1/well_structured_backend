
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface ISettingsRepository
    {
        Task<List<ConfigurationResponse>> GetConfigListAsync(Guid userId, string config);
        Task<ConfigurationResponse> GetConfigDetailsAsync(Guid id, string config);
        Task<Guid> AddConfigAsync(ConfigurationRequest Config, Guid userId, string config);
        Task<bool> UpdateConfigAsync(ConfigurationRequest Config, Guid userId, string config);
        Task<bool> DeleteConfigAsync(Guid ConfigId, Guid userId, string config);
        Task<bool> DeactivateConfigAsync(Guid ConfigId, Guid userId, string config);

        Task<List<Account>> GetAllAccountsAsync();
        Task<List<Relation>> GetAllRelationsAsync();
        Task<List<OccasionType>> GetAllOccasionTypesAsync();


    }
}
