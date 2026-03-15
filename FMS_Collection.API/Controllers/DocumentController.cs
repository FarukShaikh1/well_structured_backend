// API/Controllers/DocumentController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class DocumentController(DocumentService service, AssetService assetService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Document.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await service.GetDocumentListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{documentId:guid}")]
    [RequirePermission("Document.View")]
    public async Task<IActionResult> GetDetails(Guid documentId)
    {
        var result = await service.GetDocumentDetailsAsync(documentId);
        return Ok(result);
    }

    [HttpPost("upload")]
    [RequirePermission("Document.Upload")]
    public async Task<IActionResult> UploadDocument([FromForm] DocumentRequest document)
    {
        if (document?.file == null)
            return BadRequest("No file uploaded.");

        var assetId = await assetService.UploadDocument(document, CurrentUserId);
        if (!assetId.HasValue || assetId.Value == Guid.Empty)
            return BadRequest("Issue in uploading file.");

        document.AssetId = assetId;
        var result = await service.AddDocumentAsync(document, CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{documentId:guid}/download-url")]
    [RequirePermission("Document.Download")]
    public async Task<IActionResult> GetDownloadUrl(Guid documentId)
    {
        var response = await service.GetDocumentDetailsAsync(documentId);
        var url = await service.GetDownloadSasUrl(response.Data!.OriginalPath, response.Data.DocumentName);
        return Ok(url);
    }

    [HttpPut]
    [RequirePermission("Document.Update")]
    public async Task<IActionResult> Update([FromBody] DocumentRequest document)
    {
        var result = await service.UpdateDocumentAsync(document, CurrentUserId);
        return Ok(result);
    }

    [HttpDelete("{documentId:guid}")]
    [RequirePermission("Document.Delete")]
    public async Task<IActionResult> Delete(Guid documentId)
    {
        var result = await service.DeleteDocumentAsync(documentId, CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{documentId:guid}/sas-url")]
    [RequirePermission("Document.View")]
    public async Task<IActionResult> GetSasUrl(Guid documentId)
    {
        var response = await service.GetDocumentDetailsAsync(documentId);
        var result = assetService.GetSasUrl(AppSettings.AzureStorageContainerName, response.Data!.OriginalPath);
        return Ok(result);
    }
}