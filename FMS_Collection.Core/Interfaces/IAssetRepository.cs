
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IAssetRepository
    {
        Task<List<Assets>> GetAllAsync();
        Task<AssetResponse> GetAssetDetailsAsync(Guid? assetId);
        Task<Guid?> AddAsync(AssetRequest asset, Guid userId);
        Task<Guid?> UpdateAsync(AssetRequest asset, Guid userId);
        Task<bool> DeleteAsync(Guid? assetId, Guid userId);
    }
}
