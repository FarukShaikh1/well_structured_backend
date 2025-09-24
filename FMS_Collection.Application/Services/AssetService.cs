
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Enum;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FMS_Collection.Application.Services
{
    public class AssetService
    {
        private readonly IAssetRepository _repository;
        public AssetService(IAssetRepository repository)
        {
            _repository = repository;
        }
        #region public methods
        public async Task<ServiceResponse<List<Asset>>> GetAllAssetsAsync()
        {

            var response = new ServiceResponse<List<Asset>>();
            try
            {
                var data = await _repository.GetAllAsync();
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<AssetResponse>> GetAssetDetailsAsync(Guid assetId)
        {
            var response = new ServiceResponse<AssetResponse>();
            try
            {
                var data = await _repository.GetAssetDetailsAsync(assetId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<Guid>> AddAssetAsync(AssetRequest Asset, Guid userId)
        {
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await _repository.AddAsync(Asset, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task UpdateAssetAsync(AssetRequest Asset, Guid userId)
        {
            _repository.UpdateAsync(Asset, userId);
        }
        public async Task<ServiceResponse<bool>> DeleteAssetAsync(Guid assetId, Guid userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var data = await _repository.DeleteAsync(assetId, userId);
                response.Success = true;
                response.Data = data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<Guid>> SaveFile(IFormFile file, string documentType, Guid userId, bool isNonSecuredFile = true)
        {
            string fileName = GetFileName(file, documentType);
            AssetResponse filePathResponse = await UploadDocumentToDocStore(file, documentType, fileName);
            //CREATE ASSET
            AssetRequest assetRequest = new AssetRequest();
            assetRequest.AssetType = IsImage(file.ContentType, Path.GetExtension(fileName)) ? AssetType.Image.ToString() : AssetType.Document.ToString();
            assetRequest.UploadedFileName = fileName;
            assetRequest.OriginalPath = filePathResponse.OriginalPath;
            assetRequest.ThumbnailPath = filePathResponse.ThumbnailPath;
            assetRequest.ContentType = file.ContentType;
            assetRequest.IsNonSecuredFile = isNonSecuredFile;
            //return await AddAssetAsync(assetRequest, userId);
            var response = new ServiceResponse<Guid>();
            try
            {
                var data = await AddAssetAsync(assetRequest, userId);
                response.Success = true;
                response.Data = data.Data;
                response.Message = "Transaction Records Fetched successfully";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task UpdateFile(IFormFile file, Guid userId, Guid? assetId, string? documentType = "Other")
        {
            string fileName = GetFileName(file, documentType);
            AssetResponse oldAssetDetail = await _repository.GetAssetDetailsAsync(assetId);

            AssetResponse filePathResponse = await UploadDocumentToDocStore(file, documentType, fileName);
            //UPDATE ASSET RECORD
            AssetRequest assetResponse = new AssetRequest();
            assetResponse.Id = assetId;
            assetResponse.AssetType = IsImage(file.ContentType, Path.GetExtension(fileName)) ? AssetType.Image.ToString() : AssetType.Document.ToString();
            assetResponse.OriginalPath = filePathResponse.OriginalPath;
            assetResponse.ThumbnailPath = filePathResponse.ThumbnailPath;
            assetResponse.UploadedFileName = fileName;
            assetResponse.ContentType = file.ContentType;
            //assetResponse.IsNonSecuredFile = isNonSecuredFile;
            await UpdateAssetAsync(assetResponse, userId);

            //DELETE OLD DOC ASSET FILES
            await DeleteOldDocStoreAssetFiles(oldAssetDetail);
        }

        public async Task<int> CreateThumbnails()
        {
            int count = 0;
            try
            {
                string baseAssetDirectory = AppSettings.PhysicalPathDirectory;
                string relativeDirectoryPath = string.Format("\\{0}\\", Constants.DocumentType.BIRTHDAY_PERSON_PIC);
                //string sourcePath = baseAssetDirectory + relativeDirectoryPath;
                //string destinationPath = sourcePath + Constants.DocumentType.THUMBNAILS;

                string sourcePath = "D:\\Projects\\DeployedSites\\well_structured_frontend\\assets\\ProjectAttatchments\\Birthday_Person_Pic/";
                string destinationPath = "D:\\Projects\\DeployedSites\\well_structured_frontend\\assets\\ProjectAttatchments\\Birthday_Person_Pic\\thumbnails/";

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
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max, // Maintain aspect ratio
                            Size = new Size(200, 200)
                        }));

                        // Get the file name from the path
                        string fileName = Path.GetFileName(file);
                        string thumbFileName = $"thumb_{fileName}";

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
            //UPDATE Asset SET ThumbnailPath = REPLACE(OriginalPath,'/Birthday_Person_Pic/','/Birthday_Person_Pic/thumbnails/thumb_') WHERE ThumbnailPath LIKE '%Birthday_Person_Pic%'

            return count;
        }
        #endregion


        #region private


        private async Task<AssetResponse> UploadDocumentToDocStore(IFormFile file, string documentType, string fileName)
        {
            AssetResponse filePathResponse = new AssetResponse();
            byte[] documentBytes = null;
            if (file != null)
            {
                using (Stream inputStream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await inputStream.CopyToAsync(memoryStream);
                        documentBytes = memoryStream.ToArray();
                    }
                }
                //filePathResponse.OriginalPath = await this._assetRepository.UploadFile(fileName, file.ContentType, documentBytes, documentType);
                filePathResponse.OriginalPath = await this.UploadFile(fileName, documentBytes, documentType);
                // Create and save thumbnail if it's an image
                if (IsImage(file.ContentType))
                {
                    string uploadedFileName = Path.GetFileName(filePathResponse.OriginalPath); // just the filename
                    string baseFileNameWithoutExt = Path.GetFileNameWithoutExtension(uploadedFileName);
                    string fileExtension = Path.GetExtension(uploadedFileName);

                    string thumbFileName = "thumb_" + baseFileNameWithoutExt + fileExtension;

                    string thumbnailPath = await CreateAndSaveThumbnail(thumbFileName, documentBytes, documentType);
                    filePathResponse.ThumbnailPath = thumbnailPath;
                }
            }
            return filePathResponse;
        }

        private async Task<string> CreateAndSaveThumbnail(string thumbFileName, byte[] fileBytes, string documentType)
        {
            string baseAssetDirectory = AppSettings.PhysicalPathDirectory;
            string relativeDirectoryPath = $"/{documentType}/thumbnails/";

            ValidateDirectory(baseAssetDirectory + relativeDirectoryPath);

            string thumbnailPath = baseAssetDirectory + relativeDirectoryPath + thumbFileName;

            using (var inputStream = new MemoryStream(fileBytes))
            using (var image = await Image.LoadAsync(inputStream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(200, 200) // Adjust as needed to fit 20–50 KB
                }));

                var encoder = new JpegEncoder { Quality = 60 }; // Tune quality to reduce file size

                await image.SaveAsync(thumbnailPath, encoder);
            }

            return relativeDirectoryPath + thumbFileName;
        }

        private async Task<string> UploadFile(string fileName, byte[] fileBytes, string documentType = "Other")
        {

            //string baseAssetDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\ProjectAttatchments\\";
            //string baseAssetDirectory = "D:\\Projects\\MyProj\\Project\\MyCollection\\src\\assets\\ProjectAttatchments";
            string baseAssetDirectory = AppSettings.PhysicalPathDirectory;
            //string baseAssetDirectory = AppSettings.AssetDirectory;
            string fileExtension = Path.GetExtension(fileName);

            string newFileName = Guid.NewGuid().ToString() + fileExtension;
            string relativeDirectoryPath = string.Format("/{0}/", documentType);

            this.ValidateDirectory(baseAssetDirectory + relativeDirectoryPath);

            string newFilePath = baseAssetDirectory + relativeDirectoryPath + newFileName;
            using (FileStream sourceStream = new FileStream(newFilePath, FileMode.Create))
            {
                await sourceStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
            ;

            return relativeDirectoryPath + newFileName;
        }

        private async Task<bool> DeleteOldDocStoreAssetFiles(AssetResponse assetDetail)
        {
            bool success = false;
            if (assetDetail != null)
            {
                if (!string.IsNullOrEmpty(assetDetail.OriginalPath))
                    await DeleteFileFromDocStore(assetDetail.OriginalPath);
                if (!string.IsNullOrEmpty(assetDetail.ThumbnailPath))
                    await DeleteFileFromDocStore(assetDetail.ThumbnailPath);

                success = true;
            }
            return success;
        }

        private async Task<bool> DeleteFileFromDocStore(string fileName)
        {
            bool isDeleteSuccess = false;
            await Task.Run(() =>
            {
                if (File.Exists(AppSettings.PhysicalPathDirectory + fileName))
                //if (File.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\ProjectAttatchments\\" + fileName))
                {
                    //File.Delete(Directory.GetCurrentDirectory() + "\\wwwroot\\ProjectAttatchments\\" + fileName);
                    File.Delete(AppSettings.PhysicalPathDirectory + fileName);
                    isDeleteSuccess = true;
                }
            });
            return isDeleteSuccess;
        }


        private string GetFileName(IFormFile file, string documentType = "Other")
        {
            //string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
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

            //generate display file name

            string extension = Path.GetExtension(fileName);
            fileName = Regex.Replace(documentType, @"\s+", "") + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff").Replace("/", "") + extension;

            return fileName;
        }

        private bool IsImage(string contentType, string fileExtension = null)
        {
            if (contentType.ToLower() == "image/jpg" ||
                contentType.ToLower() == "image/jpeg" ||
                contentType.ToLower() == "image/pjpeg" ||
                contentType.ToLower() == "image/gif" ||
                contentType.ToLower() == "image/x-png" ||
                contentType.ToLower() == "image/png")
            {
                return true;
            }

            //CHECK THE IMAGE EXTENSION
            if (!string.IsNullOrEmpty(fileExtension))
            {
                if (fileExtension.ToLower() == ".jpg" ||
                fileExtension.ToLower() == ".png" ||
                fileExtension.ToLower() == ".gif" ||
                fileExtension.ToLower() == ".jpeg")
                {
                    return true;
                }
            }
            return false;
        }

        private void ValidateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion
    }
}
