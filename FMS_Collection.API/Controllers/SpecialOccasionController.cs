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
        try
        {
            var result = await service.GetDayListAsync(CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{dayId:guid}")]
    [RequirePermission("SpecialOccasion.View")]
    public async Task<IActionResult> GetDetails(Guid dayId)
    {
        try
        {
            var result = await service.GetDayDetailsAsync(dayId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost]
    [RequirePermission("SpecialOccasion.Create")]
    public async Task<IActionResult> Add([FromBody] SpecialOccasionRequest request)
    {
        try
        {
            var result = await service.AddDayAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut]
    [RequirePermission("SpecialOccasion.Update")]
    public async Task<IActionResult> Update([FromBody] SpecialOccasionRequest request)
    {
        try
        {
            var result = await service.UpdateDayAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("{dayId:guid}")]
    [RequirePermission("SpecialOccasion.Delete")]
    public async Task<IActionResult> Delete(Guid dayId)
    {
        try
        {
            var result = await service.DeleteDayAsync(dayId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
