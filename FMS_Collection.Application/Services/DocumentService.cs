
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;

namespace FMS_Collection.Application.Services
{
    public class DocumentService
    {
        private readonly IDocumentRepository _repository;
        private readonly AzureBlobService _blobService;
        private readonly AssetService _assetService;
        public DocumentService(IDocumentRepository repository, AzureBlobService blobService, AssetService assetService)
        {
            _repository = repository;
            _blobService = blobService;
            _assetService = assetService;
        }

        public async Task<ServiceResponse<List<Document>>> GetAllDocumentsAsync()
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentFetchedSuccessfully
            );
        }// => 
        public async Task<ServiceResponse<List<DocumentListResponse>>> GetDocumentListAsync(Guid userId)
        {
            // Fetch data from repository
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.GetDocumentListAsync(userId),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentListFetchedSuccessfully
            );

            // Null or empty check
            if (response?.Data == null || !response.Data.Any())
                return response;

            // Replace ImagePath and ThumbnailPath with Blob SAS URLs
            foreach (var item in response.Data)
            {
                if (!string.IsNullOrEmpty(item.OriginalPath))
                {
                    item.OriginalPath = _blobService.GetBlobSasUrl(item.OriginalPath);
                }

                if (!string.IsNullOrEmpty(item.ThumbnailPath))
                {
                    item.ThumbnailPath = _blobService.GetBlobSasUrl(item.ThumbnailPath);
                }
            }

            return response;
        }

        public async Task<ServiceResponse<DocumentDetailsResponse>> GetDocumentDetailsAsync(Guid documentId)
        {
            // Fetch data from repository
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetDocumentDetailsAsync(documentId),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentListFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<Guid>> AddDocumentAsync(DocumentRequest Document, Guid DocumentId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(Document, DocumentId),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentCreatedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> UpdateDocumentAsync(DocumentRequest Document, Guid DocumentId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.UpdateAsync(Document, DocumentId),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentUpdatedSuccessfully
            );
        }

        public async Task<ServiceResponse<bool>> DeleteDocumentAsync(Guid DocumentId, Guid userId)
        {
            // first delete assets related to the selected coin/note
            DocumentDetailsResponse coinDetails = await _repository.GetDocumentDetailsAsync(DocumentId);
            var response = await _assetService.DeleteAssetAsync(coinDetails.AssetId, userId);
            if (response == null)
            {
                return await ServiceExecutor.ExecuteAsync(
                () => null,
                FMS_Collection.Core.Constants.Constants.Messages.IssueInCoinDeletionNoteCollection
            );
            }
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(DocumentId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.DocumentDeletedSuccessfully
            );
        }
    }
}
