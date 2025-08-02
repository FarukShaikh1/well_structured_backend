using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class SettingsService
    {
        private readonly ISettingsRepository _repository;
        public SettingsService(ISettingsRepository repository)
        {
            _repository = repository;
        }

        public Task<List<ConfigurationResponse>> GetConfigListAsync(Guid userId, string config) => _repository.GetConfigListAsync(userId, config);
        public Task<ConfigurationResponse> GetConfigDetailsAsync(Guid id, string config) => _repository.GetConfigDetailsAsync(id, config);
        public Task<Guid> AddConfigAsync(ConfigurationRequest Config, Guid userId, string config) => _repository.AddConfigAsync(Config, userId, config);
        public Task<bool> UpdateConfigAsync(ConfigurationRequest Config, Guid userId, string config) => _repository.UpdateConfigAsync(Config, userId, config);
        public Task<bool> DeleteConfigAsync(Guid id, Guid userId, string config) => _repository.DeleteConfigAsync(id, userId, config);

        public Task<List<Account>> GetAllAccountsAsync() => _repository.GetAllAccountsAsync();
        public Task<List<Relation>> GetAllRelationsAsync() => _repository.GetAllRelationsAsync();
        public Task<List<OccasionType>> GetAllOccasionTypesAsync() => _repository.GetAllOccasionTypesAsync();
    }
}
