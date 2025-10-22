using FMS_Collection.Core.Common;
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

        public async Task<ServiceResponse<List<ConfigurationResponse>>> GetConfigListAsync(Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetConfigListAsync(userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationsFetchedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<List<ConfigurationResponse>>> GetActiveConfigListAsync(Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetActiveConfigListAsync(userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ActiveConfigurationsFetchedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<ConfigurationResponse>> GetConfigDetailsAsync(Guid id, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetConfigDetailsAsync(id, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDetailsFetchedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<Guid>> AddConfigAsync(ConfigurationRequest Config, Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddConfigAsync(Config, userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationCreatedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<bool>> UpdateConfigAsync(ConfigurationRequest Config, Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateConfigAsync(Config, userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationUpdatedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<bool>> DeleteConfigAsync(Guid id, Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteConfigAsync(id, userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDeletedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<bool>> DeactivateConfigAsync(Guid id, Guid userId, string config)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeactivateConfigAsync(id, userId, config),
                FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDeactivatedSuccessfully
            );
        }
        //=> 

        public async Task<ServiceResponse<List<Account>>> GetAllAccountsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAccountsAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.AccountsFetchedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<List<Relation>>> GetAllRelationsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllRelationsAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.RelationsFetchedSuccessfully
            );
        }
        //=> 
        public async Task<ServiceResponse<List<OccasionType>>> GetAllOccasionTypesAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllOccasionTypesAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.OccasionTypesFetchedSuccessfully
            );
        }
        //=> 
    }
}
