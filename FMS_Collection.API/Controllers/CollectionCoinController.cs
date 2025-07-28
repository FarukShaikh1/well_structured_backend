// API/Controllers/CoinNoteCollectionController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CoinNoteCollectionController : ControllerBase
{
    private readonly CoinNoteCollectionService _service;

    public CoinNoteCollectionController(CoinNoteCollectionService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllCoinNoteCollectionsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList(Guid userId)
    {
        var result = await _service.GetCoinNoteCollectionListAsync(userId);
        return Ok(result);
    }


    [HttpPost]
    [Route("GetSummary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _service.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid coinNoteCollectionId, Guid userId)
    {
        var result = await _service.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId,userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(CoinNoteCollectionRequest coinnotecollection, Guid userId)
    {
        var result = await _service.AddCoinNoteCollectionAsync(coinnotecollection, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(CoinNoteCollectionRequest coinnotecollection, Guid userId)
    {
        var result = await _service.UpdateCoinNoteCollectionAsync(coinnotecollection,userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid coinNoteCollectionId, Guid userId)
    {
        var result = await _service.DeleteCoinNoteCollectionAsync(coinNoteCollectionId, userId);
        return Ok(result);
    }
}