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
            var response = new ServiceResponse<List<ConfigurationResponse>>();
            try
            {
                var data = await _repository.GetConfigListAsync(userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<List<ConfigurationResponse>>> GetActiveConfigListAsync(Guid userId, string config)
        {
            var response = new ServiceResponse<List<ConfigurationResponse>>();
            try
            {
                var data = await _repository.GetActiveConfigListAsync(userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ActiveConfigurationsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<ConfigurationResponse>> GetConfigDetailsAsync(Guid id, string config)
        {
            var response = new ServiceResponse<ConfigurationResponse>();
            try
            {
                var data = await _repository.GetConfigDetailsAsync(id, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDetailsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<Guid>> AddConfigAsync(ConfigurationRequest Config, Guid userId, string config)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddConfigAsync(Config, userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationCreatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<bool>> UpdateConfigAsync(ConfigurationRequest Config, Guid userId, string config)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateConfigAsync(Config, userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<bool>> DeleteConfigAsync(Guid id, Guid userId, string config)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteConfigAsync(id, userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDeletedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<bool>> DeactivateConfigAsync(Guid id, Guid userId, string config)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeactivateConfigAsync(id, userId, config);
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.ConfigurationDeactivatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 

        public async Task<ServiceResponse<List<Account>>> GetAllAccountsAsync()
        {
            var response = new ServiceResponse<List<Account>>();
            try
            {
                var data = await _repository.GetAllAccountsAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.AccountsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<List<Relation>>> GetAllRelationsAsync()
        {
            var response = new ServiceResponse<List<Relation>>();
            try
            {
                var data = await _repository.GetAllRelationsAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.RelationsFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
        public async Task<ServiceResponse<List<OccasionType>>> GetAllOccasionTypesAsync()
        {
            var response = new ServiceResponse<List<OccasionType>>();
            try
            {
                var data = await _repository.GetAllOccasionTypesAsync();
                response.Success = true;
                response.Data = data;
                response.Message = FMS_Collection.Core.Constants.Constants.Messages.OccasionTypesFetchedSuccessfully;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        //=> 
    }
}
