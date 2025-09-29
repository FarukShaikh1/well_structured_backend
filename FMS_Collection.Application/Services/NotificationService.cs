
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
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationsFetchedSuccessfully
            );
        }// => 
        public async Task<ServiceResponse<List<NotificationListResponse>>> GetNotificationListAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetNotificationListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationListFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<NotificationDetailsResponse>> GetNotificationDetailsAsync(Guid NotificationId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetNotificationDetailsAsync(NotificationId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationDetailsFetchedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<Guid>> AddNotificationAsync(NotificationRequest Notification,Guid NotificationId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(Notification, NotificationId),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationCreatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> UpdateNotificationAsync(NotificationRequest Notification, Guid NotificationId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(Notification, NotificationId),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationUpdatedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<bool>> DeleteNotificationAsync(Guid NotificationId, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(NotificationId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationDeletedSuccessfully
            );
        }
        
        public async Task<ServiceResponse<List<NotificationSummaryResponse>>> GetSummaryAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetSummaryAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.NotificationSummaryFetchedSuccessfully
            );
        }
        
    }
}
