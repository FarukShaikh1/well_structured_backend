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
    [AllowAnonymous]
    [HttpGet("{assetId:guid}")]
    //[RequirePermission("Asset.View")]
    public async Task<IActionResult> GetDetails(Guid assetId)
    {
        try
        {
            var result = await service.GetAssetDetailsAsync(assetId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost]
    [RequirePermission("Asset.Create")]
    public async Task<IActionResult> Add([FromBody] AssetRequest asset)
    {
        try
        {
            await service.AddAssetAsync(asset, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut]
    [RequirePermission("Asset.Update")]
    public async Task<IActionResult> Update([FromBody] AssetRequest asset)
    {
        try
        {
            await service.UpdateAssetAsync(asset, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("{assetId:guid}")]
    [RequirePermission("Asset.Delete")]
    public async Task<IActionResult> Delete(Guid assetId)
    {
        try
        {
            var result = await service.DeleteAssetAsync(assetId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("upload")]
    [RequirePermission("Asset.Upload")]
    public async Task<IActionResult> UploadAndSaveFile(IFormFile file, Guid? assetId = null, string? documentType = null)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("download")]
    //[RequirePermission("Asset.Download")]
    [RequirePermission("Asset.View")]
    public async Task<IActionResult> DownloadFile([FromQuery] string imagePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return BadRequest("Invalid blob path.");

            byte[] fileBytes = await service.DownloadFileAsync(imagePath);
            if (fileBytes == null || fileBytes.Length == 0)
                return NotFound("File not found.");

            return File(fileBytes, "application/octet-stream", Path.GetFileName(imagePath));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("download-folder")]
    [RequirePermission("Asset.View")]
    //[RequirePermission("Asset.Download")]
    public async Task<IActionResult> DownloadZip([FromQuery] string folderPath)
    {
        try
        {
            byte[] zipBytes = await service.DownloadFolderAsZipAsync(folderPath);
            if (zipBytes == null || zipBytes.Length == 0)
                return NotFound("No files found in folder.");

            return File(zipBytes, "application/zip", $"{folderPath.Replace("/", "_")}.zip");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
