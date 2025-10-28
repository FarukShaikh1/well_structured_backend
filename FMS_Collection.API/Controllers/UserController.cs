// API/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList(Guid accessedBy)
    {
        var result = await _service.GetUserListAsync(accessedBy);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid userId)
    {
        var result = await _service.GetUserDetailsAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(UserRequest user, Guid createdBy)
    {
        await _service.AddUserAsync(user, createdBy);
        return Ok();
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UserRequest user, Guid updatedBy)
    {
        await _service.UpdateUserAsync(user, updatedBy);
        return Ok();
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _service.DeleteUserAsync(userId);
        return Ok();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginRequest user)
    {
        var result = await _service.GetLoginDetails(user);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetUserPermission")]
    public async Task<IActionResult> GetUserPermission(Guid userId)
    {
        var result = await _service.GetUserPermissionListAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("UpdateUserPermission")]
    public async Task<IActionResult> UpdateUserPermission(UserPermissionRequest userPermission, Guid userId)
    {
        var result = await _service.UpdateUserPermissionAsync(userPermission, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePassword request)
    {
        var response = await _service.ChangePassword(request.OldPassword, request.NewPassword, request.UserId, request.ModifiedBy);
        return Ok(new { Success = response.Data.IsSuccess, Message = response.Data.Message });
    }

}