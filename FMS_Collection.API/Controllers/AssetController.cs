// API/Controllers/AssetController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

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
    public async Task<Guid?> UploadAndSaveFile(IFormFile file, Guid userId, Guid? assetId=null, string documentType = null)
    {
        if (file != null && assetId != null)
        {
            await _service.UpdateFile(file, userId, assetId, documentType);
        }
        else if (file != null && assetId == null)
        {
            assetId = await _service.SaveFile(file, documentType, userId, false);
        }
        return assetId;
    }

}