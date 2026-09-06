using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IUserRegistrationRepository
    {
        Task<Guid> AddAsync(UserRegistration registration, string otp);
        Task<UserRegistrationResponse?> GetDetailsAsync(Guid registrationId);
        Task<bool> VerifyEmailAsync(Guid registrationId, string otp);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PendingRegistrationExistsAsync(string email);
        Task<List<UserRegistrationResponse>> GetPendingApprovalAsync();
        Task<Guid> AddAsync(UserRegistration registration);
        Task<UserRegistrationResponse?> GetByIdAsync(Guid registrationId);
        Task<UserRegistrationResponse?> GetByEmailAsync(string email);
        Task<List<UserRegistrationResponse>> GetPendingAsync();
        Task<Guid?> ApproveAsync(Guid registrationId, Guid approvedBy);
        Task<bool> RejectAsync(Guid registrationId, string reason, Guid rejectedBy);
    }
}