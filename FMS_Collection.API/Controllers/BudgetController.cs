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
public class BudgetController(IBudgetRepository budgetRepository) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var result = await budgetRepository.GetByUserAsync(CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{budgetId:guid}")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetDetails(Guid budgetId)
    {
        try
        {
            var result = await budgetRepository.GetDetailsAsync(budgetId);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost]
    [RequirePermission("Transaction.Create")]
    public async Task<IActionResult> Add([FromBody] Budget budget)
    {
        try
        {
            var newId = await budgetRepository.AddAsync(budget, CurrentUserId);
            return Ok(newId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut]
    [RequirePermission("Transaction.Update")]
    public async Task<IActionResult> Update([FromBody] Budget budget)
    {
        try
        {
            await budgetRepository.UpdateAsync(budget, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("{budgetId:guid}")]
    [RequirePermission("Transaction.Delete")]
    public async Task<IActionResult> Delete(Guid budgetId)
    {
        try
        {
            await budgetRepository.DeleteAsync(budgetId, CurrentUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
