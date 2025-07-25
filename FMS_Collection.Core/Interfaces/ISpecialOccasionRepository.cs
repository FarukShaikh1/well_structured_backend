
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface ISpecialOccasionRepository
    {
        Task<List<SpecialOccasion>> GetAllAsync();
        Task<List<SpecialOccasionListResponse>> GetDayListAsync(Guid userId);
        Task<SpecialOccasionDetailsResponse> GetDayDetailsAsync(Guid dayId, Guid userId);
        Task<Guid> AddAsync(SpecialOccasionRequest day, Guid userId);
        Task<bool> UpdateAsync(SpecialOccasionRequest day, Guid userId);
        Task<bool> DeleteAsync(Guid dayId, Guid userId);
    }
}
