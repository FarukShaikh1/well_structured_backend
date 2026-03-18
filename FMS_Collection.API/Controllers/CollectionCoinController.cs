// API/Controllers/CoinNoteCollectionController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CoinNoteCollectionController(CoinNoteCollectionService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    //[RequirePermission("Collection.View")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var result = await service.GetCoinNoteCollectionListAsync(CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var result = await service.GetCoinNoteCollectionListAsync(Guid.Empty);
                return Ok(result);
            }
            catch (Exception ex1)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }

    [HttpGet("summary")]
    //[RequirePermission("Collection.View")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var result = await service.GetSummaryAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{coinNoteCollectionId:guid}")]
    //[RequirePermission("Collection.View")]
    public async Task<IActionResult> GetDetails(Guid coinNoteCollectionId)
    {
        try
        {
            var result = await service.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var result = await service.GetCoinNoteCollectionDetailsAsync(coinNoteCollectionId, Guid.Empty);
                return Ok(result);
            }
            catch (Exception ex1)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
    [Authorize]
    [HttpPost]
    [RequirePermission("Collection.Create")]
    public async Task<IActionResult> Add([FromBody] CoinNoteCollectionRequest request)
    {
        try
        {
            var result = await service.AddCoinNoteCollectionAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPut]
    [RequirePermission("Collection.Update")]
    public async Task<IActionResult> Update([FromBody] CoinNoteCollectionRequest request)
    {
        try
        {
            var result = await service.UpdateCoinNoteCollectionAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost("update-ai-data")]
    [RequirePermission("Collection.Update")]
    public async Task<IActionResult> UpdateCoinAIData()
    {
        try
        {
            int count = await service.UpdateCoinAIData();
            return Ok(count);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpDelete("{coinNoteCollectionId:guid}")]
    [RequirePermission("Collection.Delete")]
    public async Task<IActionResult> Delete(Guid coinNoteCollectionId)
    {
        try
        {
            var result = await service.DeleteCoinNoteCollectionAsync(coinNoteCollectionId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
