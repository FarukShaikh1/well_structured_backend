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
public class SettingsController(SettingsService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("config")]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> GetConfigList([FromQuery] string config)
    {
        var result = await service.GetConfigListAsync(CurrentUserId, config);
        return Ok(result);
    }

    [HttpGet("config/active")]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> GetActiveConfigList([FromQuery] string config)
    {
        var result = await service.GetActiveConfigListAsync(CurrentUserId, config);
        return Ok(result);
    }

    [HttpGet("config/{id:guid}")]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> GetConfigDetails(Guid id, [FromQuery] string config)
    {
        var result = await service.GetConfigDetailsAsync(id, config);
        return Ok(result);
    }

    [HttpPost("config")]
    [RequirePermission("Settings.Create")]
    public async Task<IActionResult> AddConfig([FromBody] ConfigurationRequest request, [FromQuery] string config)
    {
        var result = await service.AddConfigAsync(request, CurrentUserId, config);
        return Ok(result);
    }

    [HttpPut("config")]
    [RequirePermission("Settings.Update")]
    public async Task<IActionResult> UpdateConfig([FromBody] ConfigurationRequest request, [FromQuery] string config)
    {
        var result = await service.UpdateConfigAsync(request, CurrentUserId, config);
        return Ok(result);
    }

    [HttpDelete("config/{id:guid}")]
    [RequirePermission("Settings.Delete")]
    public async Task<IActionResult> DeleteConfig(Guid id, [FromQuery] string config)
    {
        var result = await service.DeleteConfigAsync(id, CurrentUserId, config);
        return Ok(result);
    }

    [HttpPatch("config/{id:guid}/deactivate")]
    [RequirePermission("Settings.Update")]
    public async Task<IActionResult> DeactivateConfig(Guid id, [FromQuery] string config)
    {
        var result = await service.DeactivateConfigAsync(id, CurrentUserId, config);
        return Ok(result);
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccountsAll()
    {
        var result = await service.GetAllAccountsAsync();
        return Ok(result);
    }

    [HttpGet("relations")]
    public async Task<IActionResult> GetRelationsAll()
    {
        var result = await service.GetAllRelationsAsync();
        return Ok(result);
    }

    [HttpGet("occasion-types")]
    public async Task<IActionResult> GetOccasionTypesAll()
    {
        var result = await service.GetAllOccasionTypesAsync();
        return Ok(result);
    }
}