
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class CommonListService
    {
        private readonly ICommonListRepository _repository;
        public CommonListService(ICommonListRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<CommonList>>> GetAllCommonListAsync()
        {
            var response = new ServiceResponse<List<CommonList>>();
            try
            {
                var data = await _repository.GetAllCommonListAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<List<CommonListItem>>> GetAllCommonListItemAsync()
        {
            var response = new ServiceResponse<List<CommonListItem>>();
            try
            {
                var data = await _repository.GetAllCommonListItemAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        
        public async Task<ServiceResponse<List<CommonListResponse>>> GetCommonListAsync()
        {
            var response = new ServiceResponse<List<CommonListResponse>>();
            try
            {
                var data = await _repository.GetCommonListAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<List<CommonListItemResponse>>> GetCommonListItemAsync(Guid CommonId)
        {
            var response = new ServiceResponse<List<CommonListItemResponse>>();
            try
            {
                var data = await _repository.GetCommonListItemAsync(CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        
        public async Task<ServiceResponse<CommonListResponse>> GetCommonListDetailsAsync(Guid CommonId)
        {
            var response = new ServiceResponse<CommonListResponse>();
            try
            {
                var data = await _repository.GetCommonListDetailsAsync(CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<CommonListItemResponse>> GetCommonListItemDetailsAsync(Guid CommonId)
        {
            var response = new ServiceResponse<CommonListItemResponse>();
            try
            {
                var data = await _repository.GetCommonListItemDetailsAsync(CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        
        public async Task<ServiceResponse<Guid>> AddCommonListAsync(CommonListRequest Common,Guid CommonId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddCommonListAsync(Common, CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<Guid>> AddCommonListItemAsync(CommonListItemRequest Common,Guid CommonId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddCommonListItemAsync(Common, CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        
        public async Task<ServiceResponse<bool>> UpdateCommonListAsync(CommonListRequest Common, Guid CommonId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateCommonListAsync(Common, CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> UpdateCommonListItemAsync(CommonListItemRequest Common, Guid CommonId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateCommonListItemAsync(Common, CommonId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        
        public async Task<ServiceResponse<bool>> DeleteCommonListAsync(Guid CommonId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteCommonListAsync(CommonId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<bool>> DeleteCommonListItemAsync(Guid CommonId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteCommonListItemAsync(CommonId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        

        public async Task<ServiceResponse<List<CountryWithCurrency>>> GetCountryListAsync()
        {
            var response = new ServiceResponse<List<CountryWithCurrency>>();
            try
            {
                var data = await _repository.GetCountryListAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        
    }
}
