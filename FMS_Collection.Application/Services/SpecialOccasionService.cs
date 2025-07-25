
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_Collection.Application.Services
{
    public class SpecialOccasionService
    {
        private readonly ISpecialOccasionRepository _repository;
        public SpecialOccasionService(ISpecialOccasionRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SpecialOccasion>> GetAllDaysAsync() => _repository.GetAllAsync();
        public Task<List<SpecialOccasionListResponse>> GetDayListAsync(Guid userId) => _repository.GetDayListAsync(userId);
        public Task<SpecialOccasionDetailsResponse> GetDayDetailsAsync(Guid dayId, Guid userId) => _repository.GetDayDetailsAsync(dayId, userId);
        public Task<Guid> AddDayAsync(SpecialOccasionRequest Day,Guid userId) => _repository.AddAsync(Day, userId);
        public Task<bool> UpdateDayAsync(SpecialOccasionRequest Day, Guid userId) => _repository.UpdateAsync(Day, userId);
        public Task<bool> DeleteDayAsync(Guid dayId, Guid userId) => _repository.DeleteAsync(dayId, userId);
    }
}
