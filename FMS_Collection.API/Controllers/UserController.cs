// API/Controllers/UserController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController(UserService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("list")]
    [RequirePermission("User.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await service.GetUserListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    [RequirePermission("User.View")]
    public async Task<IActionResult> GetDetails(Guid userId)
    {
        var result = await service.GetUserDetailsAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> Add([FromBody] UserRequest user)
    {
        await service.AddUserAsync(user, CurrentUserId);
        return Ok();
    }

    [HttpPut]
    [RequirePermission("User.Update")]
    public async Task<IActionResult> Update([FromBody] UserRequest user)
    {
        await service.UpdateUserAsync(user, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{userId:guid}")]
    [RequirePermission("User.Delete")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        var result = await service.DeleteUserAsync(userId);
        return Ok(result);
    }

    [HttpGet("{userId:guid}/permissions")]
    [RequirePermission("User.View")]
    public async Task<IActionResult> GetUserPermission(Guid userId)
    {
        var result = await service.GetUserPermissionListAsync(userId);
        return Ok(result);
    }

    [HttpPost("{userId:guid}/permissions")]
    [RequirePermission("User.Update")]
    public async Task<IActionResult> UpdateUserPermission(Guid userId, [FromBody] UserPermissionRequest userPermission)
    {
        var result = await service.UpdateUserPermissionAsync(userPermission, CurrentUserId);
        return Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
    {
        // Extract userId from JWT — never trust request body for identity
        var response = await service.ChangePassword(request.OldPassword, request.NewPassword, CurrentUserId, CurrentUserId);
        return Ok(new { response.Data.IsSuccess, response.Data.Message });
    }

    // ── Auth endpoints — unauthenticated ──────────────────────────────────────
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await service.LoginAsync(loginRequest);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await service.RefreshTokenAsync(request);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        await service.LogoutAsync(request.RefreshToken, CurrentUserId);
        return Ok();
    }

    [HttpGet("modules")]
    public async Task<IActionResult> GetModuleList()
    {
        var result = await service.GetModuleListAsync();
        return Ok(result);
    }
}