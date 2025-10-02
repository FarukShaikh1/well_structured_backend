namespace FMS_Collection.Core.Interfaces
{
    public interface IOtpRepository
    {
        Task<Guid> SetAsync(string userKey, string otpCode, string purpose, DateTime expiresOn, Guid? createdBy);
        Task<(bool Exists, string OtpCode, DateTime ExpiresAt)> GetAsync(string identifier);
        Task InvalidateAsync(string identifier);
    }
}


