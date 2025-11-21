// API/Controllers/DocumentController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _service;
    private readonly AssetService _assetService;

    public DocumentController(DocumentService service, AssetService assetService)
    {
        _service = service;
        _assetService = assetService;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllDocumentsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList(Guid userId)
    {
        var result = await _service.GetDocumentListAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("UploadDocument")]
    public async Task<IActionResult> UploadDocument([FromForm] DocumentRequest document, Guid userId)
    {
        if (document == null || document.file == null)
            return BadRequest("No file uploaded.");

        // Upload file to asset storage
        var assetId = await _assetService.UploadDocument(document, userId);

        if (assetId == null)
            return BadRequest("Issue in uploading file.");

        // Set AssetId so service can save it
        document.AssetId = assetId;

        // Save document record
        var result = await _service.AddDocumentAsync(document, userId);

        return Ok(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(DocumentRequest Document, Guid userId)
    {
        var result = await _service.UpdateDocumentAsync(Document,userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid DocumentId, Guid userId)
    {
        var result = await _service.DeleteDocumentAsync(DocumentId, userId);
        return Ok(result);
    }
}