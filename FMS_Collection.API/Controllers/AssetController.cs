// API/Controllers/AssetController.cs
using Azure.Core;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    private readonly AssetService _service;

    public AssetController(AssetService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAssetsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAssetDetails")]
    public async Task<IActionResult> GetAssetDetails(Guid assetId)
    {
        var result = await _service.GetAssetDetailsAsync(assetId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(AssetRequest asset, Guid userId)
    {
        await _service.AddAssetAsync(asset, userId);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateAsset")]
    public async Task<IActionResult> UpdateAsset(AssetRequest asset, Guid userId)
    {
        await _service.UpdateAssetAsync(asset, userId);
        return Ok();
    }

    [HttpGet]
    [Route("DeleteAsset")]
    public async Task<IActionResult> DeleteAsset(Guid assetId, Guid userId)
    {
        var result = await _service.DeleteAssetAsync(assetId, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("UploadAndSaveFile")]
    public async Task<IActionResult> UploadAndSaveFile(IFormFile file, Guid userId, Guid? assetId = null, string documentType = null)
    {
        try
        {
            if (file != null && assetId != null)
            {
                await _service.UpdateFile(file, userId, assetId, documentType);
            }
            else if (file != null && assetId == null)
            {
                var response = await _service.SaveFile(file, documentType, userId, false);
                return Ok(response);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("CreateThumbnails")]
    public async Task<IActionResult> CreateThumbnails(string sourcePath, bool isSquare)
    {
        int total = await _service.CreateThumbnails(sourcePath, isSquare);
        return Ok(total);
    }


    [HttpGet]
    [Route("CopyFiles")]
    public async Task<IActionResult> CopyFiles(string sourcePath)
    {
        int total = await _service.CopyFiles(sourcePath);
        return Ok(total);
    }


    [HttpGet("downloadFile")]
    public async Task<IActionResult> DownloadFile(string imagePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return BadRequest("Invalid blob path.");

            byte[] fileBytes = await _service.DownloadFileAsync(imagePath);

            if (fileBytes == null || fileBytes.Length == 0)
                return NotFound("File not found.");

            string fileName = Path.GetFileName(imagePath);

            // Auto detect MIME
            string contentType = "application/octet-stream";

            return File(
                fileContents: fileBytes,
                contentType: contentType,
                fileDownloadName: fileName
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while generating zip: {ex.Message}");
        }
    }
    [HttpGet("downloadBlobZipFolder")]
    public async Task<IActionResult> DownloadZip(string containerName, string folderPath)
    {
        try
        {
            byte[] zipBytes = await _service.DownloadFolderAsZipAsync(containerName, folderPath);

            if (zipBytes == null || zipBytes.Length == 0)
                return NotFound("No files found in folder.");

            return File(
                fileContents: zipBytes,
                contentType: "application/zip",
                fileDownloadName: $"{folderPath.Replace("/", "_")}.zip"
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while generating zip: {ex.Message}");
        }
    }

    [HttpGet("UpdateBlobHeadersInFolder")]
    public async Task<IActionResult> UpdateBlobHeadersInFolderAsync(string folderPrefix)
    {
        try
        {
            await _service.UpdateBlobHeadersInFolderAsync(folderPrefix);
            return Ok("Successfully updated");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while generating zip: {ex.Message}");
        }
    }
}