using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IResponseFactory _responseFactory;

    public NotificationsController(INotificationService notificationService, IResponseFactory responseFactory)
    {
        _notificationService = notificationService;
        _responseFactory = responseFactory;
    }

    [HttpGet]
    public async Task<ActionResult> GetNotifications([FromQuery] NotificationFilterRequestDto filter)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _notificationService.GetNotificationsAsync(userId, filter);
        return Ok(_responseFactory.Success(result));
    }

    [HttpPost("{id:guid}/mark-read")]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return Ok(_responseFactory.Success(true));
    }
}
