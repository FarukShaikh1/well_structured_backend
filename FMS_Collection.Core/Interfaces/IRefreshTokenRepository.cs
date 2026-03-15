using FMS_Collection.Core.Entities;

namespace FMS_Collection.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeAsync(string token);
        Task RevokeAllForUserAsync(Guid userId);
        Task DeleteExpiredAsync();
    }
}
