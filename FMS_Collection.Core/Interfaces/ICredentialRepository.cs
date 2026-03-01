
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface ICredentialRepository
    {
        Task<List<CredentialResponse>> GetAllAsync();
        Task<List<CredentialResponse>> GetByUserAsync(Guid userId);
        Task<CredentialResponse?> GetDetailsAsync(Guid credentialId);
        Task<Guid> AddAsync(Credential credential, Guid userId);
        Task<bool> UpdateAsync(Credential credential, Guid userId);
        Task<bool> DeleteAsync(Guid credentialId, Guid userId);
    }

}
