// API/Controllers/CommonListController.cs
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
public class CommonListController(CommonListService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── CommonList endpoints ──────────────────────────────────────────────────

    [HttpGet]
    //[RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await service.GetAllCommonListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{commonListId:guid}")]
    //[RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetDetails(Guid commonListId)
    {
        try
        {
            var result = await service.GetCommonListDetailsAsync(commonListId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost]
    [RequirePermission("CommonList.Create")]
    public async Task<IActionResult> Add([FromBody] CommonListRequest request)
    {
        try
        {
            await service.AddCommonListAsync(request, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPut]
    [RequirePermission("CommonList.Update")]
    public async Task<IActionResult> Update([FromBody] CommonListRequest request)
    {
        try
        {
            await service.UpdateCommonListAsync(request, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpDelete("{commonListId:guid}")]
    [RequirePermission("CommonList.Delete")]
    public async Task<IActionResult> Delete(Guid commonListId)
    {
        try
        {
            await service.DeleteCommonListAsync(commonListId, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── CommonListItem endpoints ──────────────────────────────────────────────

    [HttpGet("items")]
    //[RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetAllItems()
    {
        try
        {
            var result = await service.GetAllCommonListItemAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("items/{commonListId:guid}")]
    //[RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetItems(Guid commonListId)
    {
        try
        {
            var result = await service.GetCommonListItemAsync(commonListId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("itemdetails/{itemId:guid}")]
    //[RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetItemDetails(Guid itemId)
    {
        try
        {
            var result = await service.GetCommonListDetailsAsync(itemId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost("items")]
    [RequirePermission("CommonList.Create")]
    public async Task<IActionResult> AddItem([FromBody] CommonListItemRequest request)
    {
        try
        {
            await service.AddCommonListItemAsync(request, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPut("items")]
    [RequirePermission("CommonList.Update")]
    public async Task<IActionResult> UpdateItem([FromBody] CommonListItemRequest request)
    {
        try
        {
            await service.UpdateCommonListItemAsync(request, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpDelete("items/{itemId:guid}")]
    [RequirePermission("CommonList.Delete")]
    public async Task<IActionResult> DeleteItem(Guid itemId)
    {
        try
        {
            await service.DeleteCommonListItemAsync(itemId, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Reference data — no permission guard needed (read-only lookup data) ──

    [HttpGet("common")]
    public async Task<IActionResult> GetCommonList()
    {
        try
        {
            var result = await service.GetCommonListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountryList()
    {
        try
        {
            var result = await service.GetCountryListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
