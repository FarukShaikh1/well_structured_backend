
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Enum;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;
using static FMS_Collection.Core.Constants.Constants;


namespace FMS_Collection.Application.Services
{
    public class AssetService
    {
        private readonly IAssetRepository _repository;
        private readonly AzureBlobService _blobService;
        public AssetService(IAssetRepository repository, AzureBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }
        #region public methods
        public async Task<ServiceResponse<List<Asset>>> GetAllAssetsAsync()
        {

            return await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAllAsync(),
                FMS_Collection.Core.Constants.Constants.Messages.AssetsFetchedSuccessfully
            );
        }

        public async Task<ServiceResponse<AssetResponse>> GetAssetDetailsAsync(Guid assetId)
        {
            var response = await ServiceExecutor.ExecuteAsync(
                () => _repository.GetAssetDetailsAsync(assetId),
                Messages.AssetDetailsFetchedSuccessfully
            );

            if (response?.Data != null)
                ApplySasUrls(response.Data);

            return response;
        }

        public async Task<ServiceResponse<Guid>> AddAssetAsync(AssetRequest Asset, Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.AddAsync(Asset, userId),
                FMS_Collection.Core.Constants.Constants.Messages.AssetCreatedSuccessfully
            );
        }

        public async Task UpdateAssetAsync(AssetRequest Asset, Guid userId)
        {
            _repository.UpdateAsync(Asset, userId);
        }
        public async Task<ServiceResponse<bool>> DeleteAssetAsync(Guid? assetId, Guid userId)
        {
            var old = await _repository.GetAssetDetailsAsync(assetId);
            await DeleteOldDocStoreAssetFiles(old);
            return await ServiceExecutor.ExecuteAsync(
                () => _repository.DeleteAsync(assetId, userId),
                FMS_Collection.Core.Constants.Constants.Messages.AssetDeletedSuccessfully
            );
        }

        public async Task<ServiceResponse<Guid>> SaveFile(IFormFile file, string folder, Guid userId, bool isNonSecuredFile = true)
        {
            var stored = await UploadDocumentToDocStore(file, folder, file.FileName);
            var request = CreateAssetRequest(stored, file, isNonSecuredFile);

            return await ServiceExecutor.ExecuteAsync(
                async () => (await AddAssetAsync(request, userId)).Data,
                Messages.AssetSavedSuccessfully
            );
        }

        public async Task UpdateFile(IFormFile file, Guid userId, Guid? assetId, string folder = "Other")
        {
            var old = await _repository.GetAssetDetailsAsync(assetId);
            await DeleteOldDocStoreAssetFiles(old);
            var stored = await UploadDocumentToDocStore(file, folder, file.FileName);
            var request = CreateAssetRequest(stored, file);

            await UpdateAssetAsync(request, userId);
        }

        public async Task<Guid?> UploadDocument(DocumentRequest document, Guid userId)
        {
            if (document.AssetId != null)
            {
                var old = await _repository.GetAssetDetailsAsync(document.AssetId);
                await DeleteOldDocStoreAssetFiles(old);
            }
            var stored = await UploadDocumentToDocStore(document.file, Constants.DocumentType.DOCUMENTS+ "/"+ userId, document.file.FileName);
            var request = CreateAssetRequest(stored, document.file);

            await UpdateAssetAsync(request, userId);
            return request.Id;
        }


        public async Task<int> CreateThumbnails(string sourcePath, bool isSquare)
        {
            string destinationPath = sourcePath + "/thumbnails";
            int count = 0;
            try
            {
                this.ValidateDirectory(sourcePath);
                this.ValidateDirectory(destinationPath);

                // Get all files from the source folder
                string[] files = Directory.GetFiles(sourcePath);

                foreach (string file in files)
                {
                    // Load the image from the file
                    using (Image image = Image.Load(file))
                    {
                        // Resize the image to create a thumbnail
                        if (isSquare)
                        {
                            // Resize and pad to 200×200 exactly square image
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(200, 200),
                                Mode = ResizeMode.Pad,
                                Position = AnchorPositionMode.Center,
                                //BackgroundColor = Color.White
                            }));
                        }
                        else
                        {
                            // Just resize directly to max 200×200 similar to original shape
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(200, 200),
                                Mode = ResizeMode.Max // maintain aspect ratio, but since square it’s fine
                            }));
                        }

                        using var outStream = new MemoryStream();
                        await image.SaveAsync(outStream, new WebpEncoder
                        {
                            Quality = 80,       // Good clarity
                            FileFormat = WebpFileFormatType.Lossy, // Balanced size
                        });

                        // Get the file name from the path
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string thumbFileName = $"thumb_{fileName}.webp"; // Save as WebP

                        // Create the full path for the thumbnail
                        string thumbnailPath = Path.Combine(destinationPath, thumbFileName);

                        // Save the thumbnail to the target folder
                        image.Save(thumbnailPath);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log any errors that occur during processing of a single file
                Console.WriteLine($"Error : {ex.Message}");
            }
            //after completing this method successfully run below query 
            //UPDATE Asset SET ThumbnailPath = REPLACE(OriginalPath,'/Birthday_Person_Pic/','/Birthday_Person_Pic/thumbnails/thumb_') WHERE OriginalPath LIKE '%Birthday_Person_Pic%'

            return count;
        }

        public async Task<int> CopyFiles(string sourcePath)
        {
            string destinationPath = sourcePath + "-copy";
            int count = 0;
            try
            {
                this.ValidateDirectory(sourcePath);
                this.ValidateDirectory(destinationPath);

                List<Asset> files = await _repository.GetAllAsync();

                foreach (var file in files)
                {
                    if (string.IsNullOrWhiteSpace(file?.OriginalPath))
                        continue;

                    // 🔹 Fix: remove leading slashes so Path.Combine works properly
                    string relativePath = file.OriginalPath.TrimStart('/', '\\');

                    string sourceFilePath = Path.Combine(sourcePath, relativePath);
                    string destinationFilePath = Path.Combine(destinationPath, relativePath);

                    if (File.Exists(sourceFilePath))
                    {
                        // 🔹 Ensure destination subfolder exists
                        string? destinationDir = Path.GetDirectoryName(destinationFilePath);
                        if (!string.IsNullOrEmpty(destinationDir))
                            this.ValidateDirectory(destinationDir);

                        // 🔹 Copy file (overwrite if already exists)
                        File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
                        Console.WriteLine($"✅ Copied: {relativePath}");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ File not found: {relativePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            return count;
        }

        public async Task<byte[]> DownloadFolderAsZipAsync(string containerName, string folderPath)
        {
            return await _blobService.DownloadFolderAsZipAsync(containerName, folderPath);
        }

        public async Task<byte[]> DownloadFileAsync(string blobPath)
        {
            return await _blobService.DownloadFileAsync(blobPath);
        }

        public async Task UpdateBlobHeadersInFolderAsync(string folderPrefix)
        {
            await _blobService.UpdateBlobHeadersInFolderAsync(folderPrefix);
        }
        #endregion


        #region private


        private async Task<AssetResponse> UploadDocumentToDocStore(IFormFile file, string folder, string fileName)
        {
            var fileBytes = await GetBytes(file);
            string originalPath = await UploadFileAsync(folder, fileName, fileBytes);

            string? thumbnailPath = IsImage(file.ContentType)
                ? await UploadThumbnailAsync(folder, originalPath, fileBytes)
                : null;

            return new AssetResponse
            {
                OriginalPath = originalPath,
                ThumbnailPath = thumbnailPath
            };
        }

        private async Task<string> UploadFileAsync(string folder, string fileName, byte[] fileBytes)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            string blobPath = $"{folder}/{uniqueFileName}";
            await _blobService.UploadFileAsync(fileBytes, blobPath);
            return blobPath;
        }

        private async Task<string> UploadThumbnailAsync(string folder, string originalBlobPath, byte[] fileBytes)
        {
            string name = Path.GetFileNameWithoutExtension(originalBlobPath);
            string thumbFileName = $"thumb_{name}.webp"; // Save as WebP

            using var inputStream = new MemoryStream(fileBytes);
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(200, 200) // Slightly bigger for better clarity
            }));

            using var outStream = new MemoryStream();
            await image.SaveAsync(outStream, new WebpEncoder
            {
                Quality = 80,       // Good clarity
                FileFormat = WebpFileFormatType.Lossy, // Balanced size
            });

            string thumbPath = $"{folder}/thumbnails/{thumbFileName}";
            await _blobService.UploadFileAsync(outStream.ToArray(), thumbPath);

            return thumbPath;
        }


        private async Task<byte[]> GetBytes(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }

        private AssetRequest CreateAssetRequest(AssetResponse storedFile, IFormFile file,bool isNonSecuredFile = true)
        {
            return new AssetRequest
            {
                Id = storedFile.Id,
                UploadedFileName = GetFileName(file),
                OriginalPath = storedFile.OriginalPath,
                ThumbnailPath = storedFile.ThumbnailPath,
                ContentType = file.ContentType,
                IsNonSecuredFile = isNonSecuredFile,
                AssetType = IsImage(file.ContentType) ? AssetType.Image.ToString() : AssetType.Document.ToString()
            };
        }

        private async Task<bool> DeleteOldDocStoreAssetFiles(AssetResponse assetDetail)
        {
            bool success = false;
            if (assetDetail != null)
            {
                if (!string.IsNullOrEmpty(assetDetail.OriginalPath))
                    await _blobService.DeleteFileAsync(assetDetail.OriginalPath);
                if (!string.IsNullOrEmpty(assetDetail.ThumbnailPath))
                    await _blobService.DeleteFileAsync(assetDetail.ThumbnailPath);
                if (!string.IsNullOrEmpty(assetDetail.PreviewPath))
                    await _blobService.DeleteFileAsync(assetDetail.PreviewPath);
                success = true;
            }
            return success;
        }


        private string GetFileName(IFormFile file, string documentType = "Other")
        {
            string fileName = file.FileName;

            if (fileName.Split('\\') != null && fileName.Split('\\').Count() > 1)
            {
                fileName = fileName.Split('\\').ToList().LastOrDefault();
            }

            //REMOVE ANY ILLEGAL CHAR IN FILE PATH
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), string.Empty);
            }
            string extension = Path.GetExtension(fileName);
            fileName = Regex.Replace(fileName, @"\s+", "") + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff").Replace("/", "") + extension;
            fileName = fileName.ToLower();
            return fileName;
        }

        private bool IsImage(string contentType, string fileExtension = null)
        {
            if (!string.IsNullOrWhiteSpace(contentType) && ImageContentTypes.Contains(contentType))
                return true;

            if (!string.IsNullOrWhiteSpace(fileExtension))
                return ImageExtensionList.Contains(fileExtension.ToLower());

            return false;
        }

        private void ValidateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void ApplySasUrls(AssetResponse asset)
        {
            if (asset == null) return;

            asset.ThumbnailPath = string.IsNullOrEmpty(asset.ThumbnailPath) ? null : _blobService.GetBlobSasUrl(asset.ThumbnailPath);
            asset.OriginalPath = string.IsNullOrEmpty(asset.OriginalPath) ? null : _blobService.GetBlobSasUrl(asset.OriginalPath);
            asset.PreviewPath = string.IsNullOrEmpty(asset.PreviewPath) ? null : _blobService.GetBlobSasUrl(asset.PreviewPath);
        }

        #endregion
    }
}
