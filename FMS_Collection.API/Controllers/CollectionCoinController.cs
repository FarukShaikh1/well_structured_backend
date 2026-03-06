// API/Controllers/CoinNoteCollectionController.cs
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
public class CoinNoteCollectionController(CoinNoteCollectionService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Collection.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await service.GetCoinNoteCollectionListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("summary")]
    [RequirePermission("Collection.View")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await service.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet("{coinNoteCollectionId:guid}")]
    [RequirePermission("Collection.View")]
    public async Task<IActionResult> GetDetails(Guid coinNoteCollectionId)
    {
        var result = await service.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, CurrentUserId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Collection.Create")]
    public async Task<IActionResult> Add([FromBody] CoinNoteCollectionRequest request)
    {
        var result = await service.AddCoinNoteCollectionAsync(request, CurrentUserId);
        return Ok(result);
    }

    [HttpPut]
    [RequirePermission("Collection.Update")]
    public async Task<IActionResult> Update([FromBody] CoinNoteCollectionRequest request)
    {
        var result = await service.UpdateCoinNoteCollectionAsync(request, CurrentUserId);
        return Ok(result);
    }

    [HttpPost("update-ai-data")]
    [RequirePermission("Collection.Update")]
    public async Task<IActionResult> UpdateCoinAIData()
    {
        int count = await service.UpdateCoinAIData();
        return Ok(count);
    }

    [HttpDelete("{coinNoteCollectionId:guid}")]
    [RequirePermission("Collection.Delete")]
    public async Task<IActionResult> Delete(Guid coinNoteCollectionId)
    {
        var result = await service.DeleteCoinNoteCollectionAsync(coinNoteCollectionId, CurrentUserId);
        return Ok(result);
    }
}