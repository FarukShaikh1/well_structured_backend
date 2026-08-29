// Core/Interfaces/IFamilyRepository.cs
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Requests;
using FMS_Collection.Core.Responses;

namespace FMS_Collection.Core.Interfaces
{
    public interface IFamilyRepository
    {
        Task<List<FamilyPersonSearchResult>> SearchPersonsAsync(string name);
        Task<FamilyGraphResponse> GetGraphAsync(Guid personId, int maxDepth = 5);
        Task<Guid>  AddPersonAsync(FamilyPersonRequest request, Guid createdBy);
        Task        UpdatePersonAsync(FamilyPersonRequest request, Guid updatedBy);
        Task        AddRelationshipAsync(FamilyRelationshipRequest request, Guid createdBy);
        Task        DeleteRelationshipAsync(Guid relationshipId);
    }
}
