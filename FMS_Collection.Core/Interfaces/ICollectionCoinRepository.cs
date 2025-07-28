
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
    public interface ICoinNoteCollectionRepository
    {
        Task<List<CoinNoteCollection>> GetAllAsync();
        Task<List<CoinNoteCollectionListResponse>> GetCoinNoteCollectionListAsync(Guid userId);
        Task<CoinNoteCollectionDetailsResponse> GetCoinNoteCollectionDetailsAsync(Guid coinNoteCollectionId, Guid userId);
        Task<Guid> AddAsync(CoinNoteCollectionRequest coinnotecollection, Guid coinNoteCollectionId);
        Task<bool> UpdateAsync(CoinNoteCollectionRequest coinnotecollection, Guid coinNoteCollectionId);
        Task<bool> DeleteAsync(Guid coinNoteCollectionId, Guid userId);
        Task<List<CoinNoteCollectionSummaryResponse>> GetSummaryAsync();
    }
}
