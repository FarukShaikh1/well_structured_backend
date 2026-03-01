
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IRoutineRepository
    {
        Task<List<Routine>> GetAllAsync();
        Task<List<Routine>> GetRoutineListAsync(Guid userId);
        Task<Routine> GetRoutineDetailsAsync(Guid RoutineId);
        Task<Guid> AddAsync(Routine Routine, Guid RoutineId);
        Task<bool> UpdateAsync(Routine Routine, Guid RoutineId);
        Task<bool> DeleteAsync(Guid RoutineId, Guid UserId);
    }
}
