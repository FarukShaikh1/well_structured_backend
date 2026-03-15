using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email, string userName, Guid? roleId, string? roleName, bool isSuperAdmin, IEnumerable<string> permissions);
        string GenerateRefreshToken();
        Guid? GetUserIdFromExpiredToken(string token);
    }
}
