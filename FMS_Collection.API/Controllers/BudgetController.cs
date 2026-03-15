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
        var result = await budgetRepository.GetByUserAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{budgetId:guid}")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetDetails(Guid budgetId)
    {
        var result = await budgetRepository.GetDetailsAsync(budgetId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Transaction.Create")]
    public async Task<IActionResult> Add([FromBody] Budget budget)
    {
        var newId = await budgetRepository.AddAsync(budget, CurrentUserId);
        return Ok(newId);
    }

    [HttpPut]
    [RequirePermission("Transaction.Update")]
    public async Task<IActionResult> Update([FromBody] Budget budget)
    {
        await budgetRepository.UpdateAsync(budget, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{budgetId:guid}")]
    [RequirePermission("Transaction.Delete")]
    public async Task<IActionResult> Delete(Guid budgetId)
    {
        await budgetRepository.DeleteAsync(budgetId, CurrentUserId);
        return Ok();
    }
}
