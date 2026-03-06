// API/Controllers/NotificationController.cs
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
public class NotificationController(NotificationService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await service.GetNotificationListAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await service.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet("{notificationId:guid}")]
    public async Task<IActionResult> GetDetails(Guid notificationId)
    {
        var result = await service.GetNotificationDetailsAsync(notificationId, CurrentUserId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] NotificationRequest notification)
    {
        var result = await service.AddNotificationAsync(notification, CurrentUserId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] NotificationRequest notification)
    {
        var result = await service.UpdateNotificationAsync(notification, CurrentUserId);
        return Ok(result);
    }

    [HttpDelete("{notificationId:guid}")]
    public async Task<IActionResult> Delete(Guid notificationId)
    {
        var result = await service.DeleteNotificationAsync(notificationId, CurrentUserId);
        return Ok(result);
    }
}