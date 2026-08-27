using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IFoodMenuRepository
    {
        Task<List<FoodMenuResponse>> GetAllAsync();

        Task<List<FoodMenuResponse>> GetByUserAsync(Guid userId);

        Task<FoodMenuResponse?> GetDetailsAsync(Guid foodMenuId);

        Task<Guid> AddAsync(FoodMenu foodMenu, Guid userId);

        Task<bool> UpdateAsync(FoodMenu foodMenu, Guid userId);

        Task<bool> DeleteAsync(Guid foodMenuId, Guid userId);
    }
}