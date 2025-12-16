using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionController(TransactionService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllTransactionsAsync();
        return Ok(response);
    }

    [HttpPost]
    [Route("GetList")]
    public async Task<IActionResult> GetList(TransactionFilterRequest filter, Guid userId)
    {
        var response = await _service.GetTransactionListAsync(filter, userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("GetSummary")]
    public async Task<IActionResult> GetSummary(TransactionFilterRequest filter, Guid userId)
    {
        var response = await _service.GetTransactionSummaryAsync(filter, userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("GetBalanceSummary")]
    public async Task<IActionResult> GetBalanceSummary(TransactionFilterRequest filter, Guid userId)
    {
        var response = await _service.GetBalanceSummaryAsync(filter, userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("GetReport")]
    public async Task<IActionResult> GetReport(TransactionFilterRequest filter, Guid userId)
    {
        var response = await _service.GetTransactionReportAsync(filter, userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("GetCategoryWiseReport")]
    public async Task<IActionResult> GetCategoryWiseReport(TransactionFilterRequest filter, Guid userId)
    {
        var response = await _service.GetCategoryWiseReportAsync(filter, userId);
        return Ok(response);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid TransactionId, Guid userId)
    {
        var response = await _service.GetTransactionDetailsAsync(TransactionId, userId);
        return Ok(response);
    }

    [HttpGet]
    [Route("GetTransactionSuggestionList")]
    public async Task<IActionResult> GetTransactionSuggestionList(Guid userId)
    {
        var response = await _service.GetTransactionSuggestionListAsync(userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(TransactionRequest Transaction, Guid userId)
    {
        var response = await _service.AddTransactionAsync(Transaction, userId);
        return Ok(response);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(TransactionRequest Transaction, Guid userId)
    {
        var response = await _service.UpdateTransactionAsync(Transaction, userId);
        return Ok(response);
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid TransactionId, Guid userId)
    {
        var response = await _service.DeleteTransactionAsync(TransactionId, userId);
        return Ok(response);
    }
}