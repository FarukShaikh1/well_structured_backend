// API/Controllers/CommonListController.cs
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
public class CommonListController(CommonListService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── CommonList endpoints ──────────────────────────────────────────────────

    [HttpGet]
    [RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllCommonListAsync();
        return Ok(result);
    }

    [HttpGet("{commonListId:guid}")]
    [RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetDetails(Guid commonListId)
    {
        var result = await service.GetCommonListDetailsAsync(commonListId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("CommonList.Create")]
    public async Task<IActionResult> Add([FromBody] CommonListRequest request)
    {
        await service.AddCommonListAsync(request, CurrentUserId);
        return Ok();
    }

    [HttpPut]
    [RequirePermission("CommonList.Update")]
    public async Task<IActionResult> Update([FromBody] CommonListRequest request)
    {
        await service.UpdateCommonListAsync(request, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{commonListId:guid}")]
    [RequirePermission("CommonList.Delete")]
    public async Task<IActionResult> Delete(Guid commonListId)
    {
        await service.DeleteCommonListAsync(commonListId, CurrentUserId);
        return Ok();
    }

    // ── CommonListItem endpoints ──────────────────────────────────────────────

    [HttpGet("items")]
    [RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetAllItems()
    {
        var result = await service.GetAllCommonListItemAsync();
        return Ok(result);
    }

    [HttpGet("{commonListId:guid}/items")]
    [RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetItems(Guid commonListId)
    {
        var result = await service.GetCommonListItemAsync(commonListId);
        return Ok(result);
    }

    [HttpGet("items/{itemId:guid}")]
    [RequirePermission("CommonList.View")]
    public async Task<IActionResult> GetItemDetails(Guid itemId)
    {
        var result = await service.GetCommonListDetailsAsync(itemId);
        return Ok(result);
    }

    [HttpPost("items")]
    [RequirePermission("CommonList.Create")]
    public async Task<IActionResult> AddItem([FromBody] CommonListItemRequest request)
    {
        await service.AddCommonListItemAsync(request, CurrentUserId);
        return Ok();
    }

    [HttpPut("items")]
    [RequirePermission("CommonList.Update")]
    public async Task<IActionResult> UpdateItem([FromBody] CommonListItemRequest request)
    {
        await service.UpdateCommonListItemAsync(request, CurrentUserId);
        return Ok();
    }

    [HttpDelete("items/{itemId:guid}")]
    [RequirePermission("CommonList.Delete")]
    public async Task<IActionResult> DeleteItem(Guid itemId)
    {
        await service.DeleteCommonListItemAsync(itemId, CurrentUserId);
        return Ok();
    }

    // ── Reference data — no permission guard needed (read-only lookup data) ──

    [HttpGet("common")]
    public async Task<IActionResult> GetCommonList()
    {
        var result = await service.GetCommonListAsync();
        return Ok(result);
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountryList()
    {
        var result = await service.GetCountryListAsync();
        return Ok(result);
    }
}
