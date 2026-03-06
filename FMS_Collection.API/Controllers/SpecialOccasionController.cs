// API/Controllers/SpecialOccasionController.cs
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
public class SpecialOccasionController(SpecialOccasionService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("SpecialOccasion.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await service.GetDayListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{dayId:guid}")]
    [RequirePermission("SpecialOccasion.View")]
    public async Task<IActionResult> GetDetails(Guid dayId)
    {
        var result = await service.GetDayDetailsAsync(dayId, CurrentUserId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("SpecialOccasion.Create")]
    public async Task<IActionResult> Add([FromBody] SpecialOccasionRequest request)
    {
        var result = await service.AddDayAsync(request, CurrentUserId);
        return Ok(result);
    }

    [HttpPut]
    [RequirePermission("SpecialOccasion.Update")]
    public async Task<IActionResult> Update([FromBody] SpecialOccasionRequest request)
    {
        var result = await service.UpdateDayAsync(request, CurrentUserId);
        return Ok(result);
    }

    [HttpDelete("{dayId:guid}")]
    [RequirePermission("SpecialOccasion.Delete")]
    public async Task<IActionResult> Delete(Guid dayId)
    {
        var result = await service.DeleteDayAsync(dayId, CurrentUserId);
        return Ok(result);
    }
}