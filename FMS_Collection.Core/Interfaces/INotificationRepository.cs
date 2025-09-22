using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllAsync();
        Task<List<NotificationListResponse>> GetNotificationListAsync(Guid userId);
        Task<NotificationDetailsResponse> GetNotificationDetailsAsync(Guid NotificationId, Guid userId);
        Task<Guid> AddAsync(NotificationRequest Notification, Guid NotificationId);
        Task<bool> UpdateAsync(NotificationRequest Notification, Guid NotificationId);
        Task<bool> DeleteAsync(Guid NotificationId, Guid userId);
        Task<List<NotificationSummaryResponse>> GetSummaryAsync();
    }
}
