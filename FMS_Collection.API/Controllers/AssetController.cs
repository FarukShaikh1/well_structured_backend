// API/Controllers/AssetController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AssetController(AssetService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{assetId:guid}")]
    [RequirePermission("Asset.View")]
    public async Task<IActionResult> GetDetails(Guid assetId)
    {
        var result = await service.GetAssetDetailsAsync(assetId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Asset.Create")]
    public async Task<IActionResult> Add([FromBody] AssetRequest asset)
    {
        await service.AddAssetAsync(asset, CurrentUserId);
        return Ok();
    }

    [HttpPut]
    [RequirePermission("Asset.Update")]
    public async Task<IActionResult> Update([FromBody] AssetRequest asset)
    {
        await service.UpdateAssetAsync(asset, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{assetId:guid}")]
    [RequirePermission("Asset.Delete")]
    public async Task<IActionResult> Delete(Guid assetId)
    {
        var result = await service.DeleteAssetAsync(assetId, CurrentUserId);
        return Ok(result);
    }

    [HttpPost("upload")]
    [RequirePermission("Asset.Upload")]
    public async Task<IActionResult> UploadAndSaveFile(IFormFile file, Guid? assetId = null, string? documentType = null)
    {
        if (file == null) return BadRequest("No file provided.");

        if (assetId.HasValue)
        {
            var response = await service.UpdateFile(file, CurrentUserId, assetId, documentType);
            return Ok(response);
        }
        else
        {
            var response = await service.SaveFile(file, documentType, CurrentUserId, false);
            return Ok(response);
        }
    }

    [HttpGet("download")]
    [RequirePermission("Asset.Download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return BadRequest("Invalid blob path.");

        byte[] fileBytes = await service.DownloadFileAsync(imagePath);
        if (fileBytes == null || fileBytes.Length == 0)
            return NotFound("File not found.");

        return File(fileBytes, "application/octet-stream", Path.GetFileName(imagePath));
    }

    [HttpGet("download-folder")]
    [RequirePermission("Asset.Download")]
    public async Task<IActionResult> DownloadZip([FromQuery] string containerName, [FromQuery] string folderPath)
    {
        byte[] zipBytes = await service.DownloadFolderAsZipAsync(containerName, folderPath);
        if (zipBytes == null || zipBytes.Length == 0)
            return NotFound("No files found in folder.");

        return File(zipBytes, "application/zip", $"{folderPath.Replace("/", "_")}.zip");
    }
}