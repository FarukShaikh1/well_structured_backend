using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RoutineController : ControllerBase
{
    private readonly IRoutineRepository _routineRepository;

    public RoutineController(IRoutineRepository routineRepository)
    {
        _routineRepository = routineRepository;
    }

    // ==============================
    // GET: api/routine
    // ==============================
    [HttpGet]
    public async Task<ActionResult<List<Routine>>> GetAll()
    {
        var result = await _routineRepository.GetAllAsync();
        return Ok(result);
    }

    // ==============================
    // GET: api/routine/user/{userId}
    // ==============================
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Routine>>> GetByUser(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Invalid UserId");

        var result = await _routineRepository.GetRoutineListAsync(userId);
        return Ok(result);
    }

    // ==============================
    // GET: api/routine/{routineId}
    // ==============================
    [HttpGet("{routineId}")]
    public async Task<ActionResult<Routine>> GetDetails(Guid routineId)
    {
        if (routineId == Guid.Empty)
            return BadRequest("Invalid RoutineId");

        var result = await _routineRepository.GetRoutineDetailsAsync(routineId);

        if (result == null)
            return NotFound("Routine not found");

        return Ok(result);
    }

    // ==============================
    // POST: api/routine
    // ==============================
    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] Routine routine)
    {
        if (routine == null)
            return BadRequest("Routine data is required");

        if (routine.UserId == Guid.Empty)
            return BadRequest("UserId is required");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = routine.UserId;

        var newId = await _routineRepository.AddAsync(routine, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Routine entry added successfully."
        });
    }

    // ==============================
    // PUT: api/routine/{routineId}
    // ==============================
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Routine routine)
    {
        if (routine == null)
            return BadRequest("Routine data is required");

        if (routine.Id == Guid.Empty)
            return BadRequest("Invalid RoutineId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = routine.UserId;

        await _routineRepository.UpdateAsync(routine, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Routine updated successfully"
        });
    }

    // ==============================
    // PUT: api/routine/{routineId}
    // ==============================
    [HttpGet("Delete")]
    public async Task<ActionResult> Delete(Guid routineId, Guid UserId)
    {

        if (routineId == Guid.Empty)
            return BadRequest("Invalid routineId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = UserId;

        await _routineRepository.DeleteAsync(routineId, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Routine Entry Deleted successfully."
        });
    }

}
