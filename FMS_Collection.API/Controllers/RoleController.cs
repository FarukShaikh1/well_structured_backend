// API/Controllers/RoleController.cs
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
public class RoleController(RoleService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Role.View")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var result = await service.GetRoleListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{roleId:guid}")]
    [RequirePermission("Role.View")]
    public async Task<IActionResult> GetDetails(Guid roleId)
    {
        try
        {
            var result = await service.GetRoleDetailsAsync(roleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost]
    [RequirePermission("Role.Create")]
    public async Task<IActionResult> Add([FromBody] RoleRequest role)
    {
        try
        {
            await service.AddRoleAsync(role, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut]
    [RequirePermission("Role.Update")]
    public async Task<IActionResult> Update([FromBody] RoleRequest role)
    {
        try
        {
            await service.UpdateRoleAsync(role, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("{roleId:guid}")]
    [RequirePermission("Role.Delete")]
    public async Task<IActionResult> Delete(Guid roleId)
    {
        try
        {
            await service.DeleteRoleAsync(roleId, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
