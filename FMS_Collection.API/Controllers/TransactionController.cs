using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionController(TransactionService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost("list")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetList([FromBody] TransactionFilterRequest filter)
    {
        try
        {
            var response = await service.GetTransactionListAsync(filter, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("summary")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetSummary([FromBody] TransactionFilterRequest filter)
    {
        try
        {
            var response = await service.GetTransactionSummaryAsync(filter, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("balance-summary")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetBalanceSummary([FromBody] TransactionFilterRequest filter)
    {
        try
        {
            var response = await service.GetBalanceSummaryAsync(filter, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost("report")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetReport([FromBody] TransactionFilterRequest filter)
    {
        try
        {
            var response = await service.GetTransactionReportAsync(filter, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    //[HttpPost("report/budget-wise")]
    //[RequirePermission("Transaction.View")]
    //public async Task<IActionResult> GetBudgetWiseReport([FromBody] TransactionFilterRequest filter)
    //{
    //    var response = await service.GetBudgetWiseReportAsync(filter, CurrentUserId);
    //    return Ok(response);
    //}

    [HttpPost("report/category-wise")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetCategoryWiseReport([FromBody] TransactionFilterRequest filter)
    {
        try
        {
            var response = await service.GetBudgetWiseReportAsync(filter, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{transactionId:guid}")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetDetails(Guid transactionId)
    {
        try
        {
            var response = await service.GetTransactionDetailsAsync(transactionId, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("suggestions")]
    [RequirePermission("Transaction.View")]
    public async Task<IActionResult> GetSuggestionList()
    {
        try
        {
            var response = await service.GetTransactionSuggestionListAsync(CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPost]
    [RequirePermission("Transaction.Create")]
    public async Task<IActionResult> Add([FromBody] TransactionRequest transaction)
    {
        try
        {
            var response = await service.AddTransactionAsync(transaction, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut]
    [RequirePermission("Transaction.Update")]
    public async Task<IActionResult> Update([FromBody] TransactionRequest transaction)
    {
        try
        {
            var response = await service.UpdateTransactionAsync(transaction, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpDelete("{transactionId:guid}")]
    [RequirePermission("Transaction.Delete")]
    public async Task<IActionResult> Delete(Guid transactionId)
    {
        try
        {
            var response = await service.DeleteTransactionAsync(transactionId, CurrentUserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
