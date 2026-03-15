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
        var result = await service.GetRoleListAsync();
        return Ok(result);
    }

    [HttpGet("{roleId:guid}")]
    [RequirePermission("Role.View")]
    public async Task<IActionResult> GetDetails(Guid roleId)
    {
        var result = await service.GetRoleDetailsAsync(roleId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Role.Create")]
    public async Task<IActionResult> Add([FromBody] RoleRequest role)
    {
        await service.AddRoleAsync(role, CurrentUserId);
        return Ok();
    }

    [HttpPut]
    [RequirePermission("Role.Update")]
    public async Task<IActionResult> Update([FromBody] RoleRequest role)
    {
        await service.UpdateRoleAsync(role, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{roleId:guid}")]
    [RequirePermission("Role.Delete")]
    public async Task<IActionResult> Delete(Guid roleId)
    {
        await service.DeleteRoleAsync(roleId, CurrentUserId);
        return Ok();
    }
}