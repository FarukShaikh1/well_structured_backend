// API/Controllers/CommonListController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CommonListController : ControllerBase
{
    private readonly CommonListService _service;

    public CommonListController(CommonListService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllCommonListAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCommonListItemAll")]
    public async Task<IActionResult> GetCommonListItemAll()
    {
        var result = await _service.GetAllCommonListItemAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCommonList")]
    public async Task<IActionResult> GetCommonList()
    {
        var result = await _service.GetCommonListAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCountryList")]
    public async Task<IActionResult> GetCountryList()
    {
        var result = await _service.GetCountryListAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCommonListItem")]
    public async Task<IActionResult> GetCommonListItem(Guid CommonListId)
    {
        var result = await _service.GetCommonListItemAsync(CommonListId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCommonListDetails")]
    public async Task<IActionResult> GetCommonListDetails(Guid CommonId)
    {
        var result = await _service.GetCommonListDetailsAsync(CommonId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetCommonListItemDetails")]
    public async Task<IActionResult> GetCommonListItemDetails(Guid CommonId)
    {
        var result = await _service.GetCommonListDetailsAsync(CommonId);
        return Ok(result);
    }

    [HttpPost]
    [Route("AddCommonList")]
    public async Task<IActionResult> AddCommonList(CommonListRequest Common, Guid createdBy)
    {
        await _service.AddCommonListAsync(Common, createdBy);
        return Ok();
    }

    [HttpPost]
    [Route("AddCommonListItem")]
    public async Task<IActionResult> AddCommonListItem(CommonListItemRequest Common, Guid createdBy)
    {
        await _service.AddCommonListItemAsync(Common, createdBy);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateCommonList")]
    public async Task<IActionResult> UpdateCommonList(CommonListRequest Common, Guid updatedBy)
    {
        await _service.UpdateCommonListAsync(Common, updatedBy);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateCommonListItem")]
    public async Task<IActionResult> UpdateCommonListItem(CommonListItemRequest Common, Guid userId)
    {
        await _service.UpdateCommonListItemAsync(Common, userId);
        return Ok();
    }

    [HttpGet]
    [Route("DeleteCommonList")]
    public async Task<IActionResult> DeleteCommonList(Guid CommonId, Guid userId)
    {
        await _service.DeleteCommonListAsync(CommonId, userId);
        return Ok();
    }

    [HttpGet]
    [Route("DeleteICommonListItem")]
    public async Task<IActionResult> DeleteCommonListItem(Guid CommonId, Guid userId)
    {
        await _service.DeleteCommonListItemAsync(CommonId, userId);
        return Ok();
    }
}