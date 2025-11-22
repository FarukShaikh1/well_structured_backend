
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Core.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllAsync();
        Task<List<DocumentListResponse>> GetDocumentListAsync(Guid userId);
        Task<DocumentDetailsResponse> GetDocumentDetailsAsync(Guid DocumentId);
        Task<Guid> AddAsync(DocumentRequest Document, Guid DocumentId);
        Task<bool> UpdateAsync(DocumentRequest Document, Guid DocumentId);
        Task<bool> DeleteAsync(Guid DocumentId, Guid userId);
    }
}
