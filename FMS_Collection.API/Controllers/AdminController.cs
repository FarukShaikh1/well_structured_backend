// API/Controllers/AdminController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

/// <summary>SuperAdmin-only management endpoints for permissions, role assignments, and audit logs.</summary>
[ApiController]
[Authorize]
[RequirePermission("Admin.Access")]
[Route("api/[controller]")]
[Produces("application/json")]
public class AdminController(AdminService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── Permissions ───────────────────────────────────────────────────────────

    [HttpGet("permissions")]
    public async Task<IActionResult> GetAllPermissions()
    {
        try
        {
            var result = await service.GetAllPermissionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("permissions/role/{roleId:guid}")]
    public async Task<IActionResult> GetPermissionsByRole(Guid roleId)
    {
        try
        {
            var result = await service.GetPermissionsByRoleAsync(roleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("permissions")]
    public async Task<IActionResult> AddPermission([FromBody] PermissionRequest request)
    {
        try
        {
            var result = await service.AddPermissionAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut("permissions/{permissionId:guid}")]
    public async Task<IActionResult> UpdatePermission(Guid permissionId, [FromBody] PermissionRequest request)
    {
        try
        {
            var result = await service.UpdatePermissionAsync(permissionId, request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("permissions/{permissionId:guid}")]
    public async Task<IActionResult> DeletePermission(Guid permissionId)
    {
        try
        {
            var result = await service.DeletePermissionAsync(permissionId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Role–Permission assignment ─────────────────────────────────────────────

    [HttpPost("roles/permissions")]
    public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignRolePermissionsRequest request)
    {
        try
        {
            var result = await service.AssignPermissionsToRoleAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("roles/{roleId:guid}/permissions/{permissionId:guid}")]
    public async Task<IActionResult> RevokePermissionFromRole(Guid roleId, Guid permissionId)
    {
        try
        {
            var result = await service.RevokePermissionFromRoleAsync(roleId, permissionId, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Audit logs ────────────────────────────────────────────────────────────

    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] Guid? userId = null,
        [FromQuery] string? action = null)
    {
        try
        {
            var result = await service.GetAuditLogsAsync(pageNumber, pageSize, userId, action);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("audit-logs/count")]
    public async Task<IActionResult> GetAuditLogCount(
        [FromQuery] Guid? userId = null,
        [FromQuery] string? action = null)
    {
        try
        {
            var result = await service.GetAuditLogCountAsync(userId, action);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
