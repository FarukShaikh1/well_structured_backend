using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _service;

    public ExpenseController(ExpenseService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        
        var result = await _service.GetAllExpensesAsync();
        return Ok(result);
    }

    [HttpPost]
    [Route("GetList")]
    public async Task<IActionResult> GetList(ExpenseFilterRequest filter, Guid userId)
    {
        var result = await _service.GetExpenseListAsync(filter, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("GetSummary")]
    public async Task<IActionResult> GetSummary(ExpenseFilterRequest filter, Guid userId)
    {
        var result = await _service.GetExpenseSummaryAsync(filter, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("GetReport")]
    public async Task<IActionResult> GetReport(ExpenseFilterRequest filter, Guid userId)
    {
        var result = await _service.GetExpenseReportAsync(filter, userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid expenseId, Guid userId)
    {
        var result = await _service.GetExpenseDetailsAsync(expenseId, userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetExpenseSuggestionList")]
    public async Task<IActionResult> GetExpenseSuggestionList(Guid userId)
    {
        var result = await _service.GetExpenseSuggestionListAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(ExpenseRequest expense, Guid userId)
    {
        var result = await _service.AddExpenseAsync(expense, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ExpenseRequest expense, Guid userId)
    {
        var result = await _service.UpdateExpenseAsync(expense,userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid expenseId, Guid userId)
    {
        var result = await _service.DeleteExpenseAsync(expenseId, userId);
        return Ok(result);
    }
}