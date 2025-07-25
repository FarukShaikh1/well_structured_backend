// API/Controllers/DayController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SpecialOccasionController : ControllerBase
{
    private readonly SpecialOccasionService _service;

    public SpecialOccasionController(SpecialOccasionService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllDaysAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList(Guid userId)
    {
        var result = await _service.GetDayListAsync(userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid dayId, Guid userId)
    {
        var result = await _service.GetDayDetailsAsync(dayId, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add([FromBody] SpecialOccasionRequest specialOccasionRequest, Guid userId)
    {
        var result = await _service.AddDayAsync(specialOccasionRequest, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update([FromBody] SpecialOccasionRequest specialOccasionRequest, Guid userId)
    {
        var result = await _service.UpdateDayAsync(specialOccasionRequest,userId);
        return Ok();
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid dayId, Guid userId)
    {
        var result = await _service.DeleteDayAsync(dayId, userId);
        return Ok(result);
    }
}