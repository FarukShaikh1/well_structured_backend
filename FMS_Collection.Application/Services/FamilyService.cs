// Application/Services/FamilyService.cs
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Requests;
using FMS_Collection.Core.Responses;

namespace FMS_Collection.Application.Services
{
    public class FamilyService(IFamilyRepository repository)
    {
        public async Task<ServiceResponse<List<FamilyPersonSearchResult>>> SearchPersonsAsync(string name)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.SearchPersonsAsync(name),
                "Persons fetched successfully."
            );
        }

        public async Task<ServiceResponse<FamilyGraphResponse>> GetGraphAsync(Guid personId, int maxDepth = 5)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.GetGraphAsync(personId, maxDepth),
                "Family graph loaded successfully."
            );
        }

        public async Task<ServiceResponse<Guid>> AddPersonAsync(FamilyPersonRequest request, Guid createdBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.AddPersonAsync(request, createdBy),
                "Person added successfully."
            );
        }

        public async Task<ServiceResponse<bool>> UpdatePersonAsync(FamilyPersonRequest request, Guid updatedBy)
        {
            return await ServiceExecutor.ExecuteAsync<bool>(async () =>
            {
                await repository.UpdatePersonAsync(request, updatedBy);
                return true;
            }, "Person updated successfully.");
        }

        public async Task<ServiceResponse<bool>> AddRelationshipAsync(FamilyRelationshipRequest request, Guid createdBy)
        {
            return await ServiceExecutor.ExecuteAsync<bool>(async () =>
            {
                await repository.AddRelationshipAsync(request, createdBy);
                return true;
            }, "Relationship added successfully.");
        }

        public async Task<ServiceResponse<bool>> DeleteRelationshipAsync(Guid relationshipId)
        {
            return await ServiceExecutor.ExecuteAsync<bool>(async () =>
            {
                await repository.DeleteRelationshipAsync(relationshipId);
                return true;
            }, "Relationship removed successfully.");
        }
    }
}
