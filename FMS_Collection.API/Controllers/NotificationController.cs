// API/Controllers/NotificationController.cs
using Microsoft.AspNetCore.Mvc;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;

namespace FMS_Collection.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _service;

    public NotificationController(NotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllNotificationsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<IActionResult> GetList(Guid userId)
    {
        var result = await _service.GetNotificationListAsync(userId);
        return Ok(result);
    }


    [HttpPost]
    [Route("GetSummary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _service.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("GetDetails")]
    public async Task<IActionResult> GetDetails(Guid NotificationId, Guid userId)
    {
        var result = await _service.GetNotificationDetailsAsync(NotificationId, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> Add(NotificationRequest Notification, Guid userId)
    {
        var result = await _service.AddNotificationAsync(Notification, userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(NotificationRequest Notification, Guid userId)
    {
        var result = await _service.UpdateNotificationAsync(Notification, userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(Guid NotificationId, Guid userId)
    {
        var result = await _service.DeleteNotificationAsync(NotificationId, userId);
        return Ok(result);
    }
}