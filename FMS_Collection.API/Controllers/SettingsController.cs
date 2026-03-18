using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
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
        try
        {
            var result = await service.GetConfigListAsync(CurrentUserId, config);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("config/active")]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> GetActiveConfigList([FromQuery] string config, string? userId)
    {
        try
        {
            ServiceResponse<List<ConfigurationResponse>> result = new ServiceResponse<List<ConfigurationResponse>>();
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid selectedUserId))
            {
                result = await service.GetActiveConfigListAsync(selectedUserId, config);
            }
            else
            {
                result = await service.GetActiveConfigListAsync(CurrentUserId, config);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("config/{id:guid}")]
    [RequirePermission("Settings.View")]
    public async Task<IActionResult> GetConfigDetails(Guid id, [FromQuery] string config)
    {
        try
        {
            var result = await service.GetConfigDetailsAsync(id, config);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("config")]
    [RequirePermission("Settings.Create")]
    public async Task<IActionResult> AddConfig([FromBody] ConfigurationRequest request, [FromQuery] string config, string? userId)
    {
        try
        {
            ServiceResponse<Guid> result = new ServiceResponse<Guid>();
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid selectedUserId))
            {
                result = await service.AddConfigAsync(request, selectedUserId, config);
            }
            else
            {
                result = await service.AddConfigAsync(request, CurrentUserId, config);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut("config")]
    [RequirePermission("Settings.Update")]
    public async Task<IActionResult> UpdateConfig([FromBody] ConfigurationRequest request, [FromQuery] string config)
    {
        try
        {
            var result = await service.UpdateConfigAsync(request, CurrentUserId, config);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("config/{id:guid}")]
    [RequirePermission("Settings.Delete")]
    public async Task<IActionResult> DeleteConfig(Guid id, [FromQuery] string config)
    {
        try
        {
            var result = await service.DeleteConfigAsync(id, CurrentUserId, config);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPatch("config/{id:guid}/deactivate")]
    [RequirePermission("Settings.Update")]
    public async Task<IActionResult> DeactivateConfig(Guid id, [FromQuery] string config)
    {
        try
        {
            var result = await service.DeactivateConfigAsync(id, CurrentUserId, config);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccountsAll()
    {
        try
        {
            var result = await service.GetAllAccountsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("relations")]
    public async Task<IActionResult> GetRelationsAll()
    {
        try
        {
            var result = await service.GetAllRelationsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("occasion-types")]
    public async Task<IActionResult> GetOccasionTypesAll()
    {
        try
        {
            var result = await service.GetAllOccasionTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
