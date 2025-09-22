
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _repository;
        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<Notification>>> GetAllNotificationsAsync()
        {
            var response = new ServiceResponse<List<Notification>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }// => 
        public async Task<ServiceResponse<List<NotificationListResponse>>> GetNotificationListAsync(Guid userId)
        {
            var response = new ServiceResponse<List<NotificationListResponse>>();
            try
            {
                var data = await _repository.GetNotificationListAsync(userId);
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
        
        public async Task<ServiceResponse<NotificationDetailsResponse>> GetNotificationDetailsAsync(Guid NotificationId, Guid userId)
        {
            var response = new ServiceResponse<NotificationDetailsResponse>();
            try
            {
                var data = await _repository.GetNotificationDetailsAsync(NotificationId, userId);
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
        
        public async Task<ServiceResponse<Guid>> AddNotificationAsync(NotificationRequest Notification,Guid NotificationId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddAsync(Notification, NotificationId);
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
        
        public async Task<ServiceResponse<bool>> UpdateNotificationAsync(NotificationRequest Notification, Guid NotificationId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.UpdateAsync(Notification, NotificationId);
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
        
        public async Task<ServiceResponse<bool>> DeleteNotificationAsync(Guid NotificationId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(NotificationId, userId);
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
        
        public async Task<ServiceResponse<List<NotificationSummaryResponse>>> GetSummaryAsync()
        {
            var response = new ServiceResponse<List<NotificationSummaryResponse>>();
            try
            {
                var data = await _repository.GetSummaryAsync();
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
