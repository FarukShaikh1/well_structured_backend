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
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController(UserService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [Authorize]
    [HttpGet("list")]
    [RequirePermission("User.View")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var result = await service.GetUserListAsync(CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    [RequirePermission("User.View")]
    public async Task<IActionResult> GetDetails(Guid userId)
    {
        try
        {
            var result = await service.GetUserDetailsAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("GetDetailsByEmail")]
    public async Task<IActionResult> GetDetailsByEmail(string email)
    {
        try
        {
            var result = await service.GetUserDetailsAsync(null,email);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> Add([FromBody] UserRequest user)
    {
        try
        {
            await service.AddUserAsync(user, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPut]
    [RequirePermission("User.Update")]
    public async Task<IActionResult> Update([FromBody] UserRequest user)
    {
        try
        {
            await service.UpdateUserAsync(user, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpDelete("{userId:guid}")]
    [RequirePermission("User.Delete")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        try
        {
            var result = await service.DeleteUserAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [AllowAnonymous]
    [HttpGet("permissions/{userId:guid}")]
    //[RequirePermission("User.View")]
    public async Task<IActionResult> GetUserPermission(Guid userId)
    {
        try
        {
            var result = await service.GetUserPermissionListAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [AllowAnonymous]
    [HttpGet("permissionsForMenu/{userId:guid}")]
    public async Task<IActionResult> GetUserPermissionForMenu(Guid userId)
    {
        try
        {
            var result = await service.GetUserPermissionListAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost("permissions")]
    [RequirePermission("User.Update")]
    public async Task<IActionResult> UpdateUserPermission([FromBody] UserPermissionRequest userPermission)
    {
        try
        {
            var result = await service.UpdateUserPermissionAsync(userPermission, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
    {
        try
        {
            // Extract userId from JWT — never trust request body for identity
            var response = await service.ChangePassword(request.OldPassword, request.NewPassword, CurrentUserId, CurrentUserId);
            return Ok(new { response.Data.Success, response.Data.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var response = await service.ForgotPassword(email);
            return Ok(new { response.Data.Success, response.Data.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Auth endpoints — unauthenticated ──────────────────────────────────────
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var result = await service.LoginAsync(loginRequest);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var result = await service.RefreshTokenAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        try
        {
            await service.LogoutAsync(request.RefreshToken, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [AllowAnonymous]
    [HttpGet("modules")]
    public async Task<IActionResult> GetModuleList()
    {
        try
        {
            var result = await service.GetModuleListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
