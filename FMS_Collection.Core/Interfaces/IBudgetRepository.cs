
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IBudgetRepository
    {
        Task<List<BudgetResponse>> GetAllAsync();
        Task<List<BudgetResponse>> GetByUserAsync(Guid userId);
        Task<BudgetResponse?> GetDetailsAsync(Guid budgetId);
        Task<Guid> AddAsync(Budget budget, Guid userId);
        Task<bool> UpdateAsync(Budget budget, Guid userId);
        Task<bool> DeleteAsync(Guid budgetId, Guid userId);
    }

}
