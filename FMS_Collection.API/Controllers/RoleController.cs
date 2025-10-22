// API/Controllers/RoleController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleService _service;

    public RoleController(RoleService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllRolesAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList()
    {
        var result = await _service.GetRoleListAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid roleId)
    {
        var result = await _service.GetRoleDetailsAsync(roleId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(RoleRequest role, Guid userId)
    {
        await _service.AddRoleAsync(role, userId);
        return Ok();
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(RoleRequest role, Guid userId)
    {
        await _service.UpdateRoleAsync(role, userId);
        return Ok();
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid roleId, Guid userId)
    {
        await _service.DeleteRoleAsync(roleId, userId);
        return Ok();
    }
}