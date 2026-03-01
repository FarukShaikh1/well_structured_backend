using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetController(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    // ==============================
    // GET: api/budget
    // ==============================
    [HttpGet]
    public async Task<ActionResult<List<Budget>>> GetAll()
    {
        var result = await _budgetRepository.GetAllAsync();
        return Ok(result);
    }

    // ==============================
    // GET: api/budget/user/{userId}
    // ==============================
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Budget>>> GetByUser(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Invalid UserId");

        var result = await _budgetRepository.GetByUserAsync(userId);
        return Ok(result);
    }

    // ==============================
    // GET: api/budget/{budgetId}
    // ==============================
    [HttpGet("{budgetId}")]
    public async Task<ActionResult<Budget>> GetDetails(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            return BadRequest("Invalid BudgetId");

        var result = await _budgetRepository.GetDetailsAsync(budgetId);

        if (result == null)
            return NotFound("Budget not found");

        return Ok(result);
    }

    // ==============================
    // POST: api/budget
    // ==============================
    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] Budget budget)
    {
        if (budget == null)
            return BadRequest("Budget data is required");

        if (budget.UserId == Guid.Empty)
            return BadRequest("UserId is required");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = budget.UserId;

        var newId = await _budgetRepository.AddAsync(budget, loggedInUserId);
        return Ok(newId);
    }

    // ==============================
    // PUT: api/budget/{budgetId}
    // ==============================
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Budget budget)
    {
        if (budget == null)
            return BadRequest("Budget data is required");

        if (budget.Id == Guid.Empty)
            return BadRequest("Invalid BudgetId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = budget.UserId;

        await _budgetRepository.UpdateAsync(budget, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Budget updated successfully"
        });
    }

    // ==============================
    // PUT: api/budget/{budgetId}
    // ==============================
    [HttpGet("Delete")]
    public async Task<ActionResult> Delete(Guid budgetId, Guid UserId)
    {

        if (budgetId == Guid.Empty)
            return BadRequest("Invalid BudgetId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = UserId;

        await _budgetRepository.DeleteAsync(budgetId, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Budget Entry Deleted successfully."
        });
    }

}
