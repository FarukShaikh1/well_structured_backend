using FMS_Collection.API.Authorization;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoutineController(IRoutineRepository routineRepository) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Routine.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await routineRepository.GetRoutineListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{routineId:guid}")]
    [RequirePermission("Routine.View")]
    public async Task<IActionResult> GetDetails(Guid routineId)
    {
        var result = await routineRepository.GetRoutineDetailsAsync(routineId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Routine.Create")]
    public async Task<IActionResult> Add([FromBody] Routine routine)
    {
        var newId = await routineRepository.AddAsync(routine, CurrentUserId);
        return Ok(newId);
    }

    [HttpPut]
    [RequirePermission("Routine.Update")]
    public async Task<IActionResult> Update([FromBody] Routine routine)
    {
        await routineRepository.UpdateAsync(routine, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{routineId:guid}")]
    [RequirePermission("Routine.Delete")]
    public async Task<IActionResult> Delete(Guid routineId)
    {
        await routineRepository.DeleteAsync(routineId, CurrentUserId);
        return Ok();
    }
}
