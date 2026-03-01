using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly SettingsService _service;

    public SettingsController(SettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetConfigList")]
    public async Task<IActionResult> GetConfigList(Guid userId, string config)
    {
        var result = await _service.GetConfigListAsync(userId, config);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetActiveConfigList")]
    public async Task<IActionResult> GetActiveConfigList(Guid userId, string config)
    {
        var result = await _service.GetActiveConfigListAsync(userId, config);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetConfigDetails")]
    public async Task<IActionResult> GetConfigDetails(Guid id, string config)
    {
        var result = await _service.GetConfigDetailsAsync(id, config);
        return Ok(result);
    }

    [HttpPost]
    [Route("AddConfig")]
    public async Task<IActionResult> AddConfig(ConfigurationRequest request, Guid userId, string config)
    {
        var result = await _service.AddConfigAsync(request, userId, config);
        return Ok(result);
    }

    [HttpPost]
    [Route("UpdateConfig")]
    public async Task<IActionResult> UpdateConfig(ConfigurationRequest request, Guid userId, string config)
    {
        var result = await _service.UpdateConfigAsync(request,userId, config);
        return Ok(result);
    }

    [HttpGet]
    [Route("DeleteConfig")]
    public async Task<IActionResult> DeleteConfig(Guid id, Guid userId, string config)
    {
        var result = await _service.DeleteConfigAsync(id, userId, config);
        return Ok(result);
    }

    [HttpGet]
    [Route("DeactiveConfig")]
    public async Task<IActionResult> DeactiveConfig(Guid id, Guid userId, string config)
    {
        var result = await _service.DeactivateConfigAsync(id, userId, config);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAccountsAll")]
    public async Task<IActionResult> GetAccountsAll()
    {
        var result = await _service.GetAllAccountsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetRelationsAll")]
    public async Task<IActionResult> GetRelationsAll()
    {
        var result = await _service.GetAllRelationsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetOccasionTypesAll")]
    public async Task<IActionResult> GetOccasionTypesAll()
    {
        var result = await _service.GetAllOccasionTypesAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetTransactionSubCategoryAll")]
    public async Task<IActionResult> GetTransactionSubCategoryAll()
    {
        var result = await _service.GetAllOccasionTypesAsync();
        return Ok(result);
    }
}