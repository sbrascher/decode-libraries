using Decode.AspNetCore.Models;
using Decode.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Decode.AspNetCore.Controllers;

/// <summary>
/// Base controller for Decode APIs, providing standardized response handling.
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{
    private IDomainNotificationContext? _notificationContext;

    /// <summary>
    /// Gets the domain notification context from the request services.
    /// </summary>
    protected IDomainNotificationContext NotificationContext =>
        _notificationContext ??= HttpContext.RequestServices.GetRequiredService<IDomainNotificationContext>();

    /// <summary>
    /// Returns a standardized API response based on the presence of domain notifications.
    /// </summary>
    /// <param name="result">The payload to return if successful.</param>
    /// <param name="statusCode">Optional status code override for success scenarios.</param>
    /// <returns>An <see cref="IActionResult"/> with the standardized response.</returns>
    protected new IActionResult Response(object? result = null, int? statusCode = null)
    {
        if (!NotificationContext.HasNotifications)
        {
            if (statusCode.HasValue)
            {
                return StatusCode(statusCode.Value, new ApiResponse(success: true, data: result));
            }

            return Ok(new ApiResponse(success: true, data: result));
        }

        int errorStatusCode = NotificationContext.GetStatusCode();
        IEnumerable<string> messages = NotificationContext.GetMessages();

        return StatusCode(errorStatusCode, new ApiResponse(
            success: false,
            data: result,
            errors: messages
        ));
    }

    /// <summary>
    /// Returns a standardized Created (201) response if no notifications are present.
    /// </summary>
    protected IActionResult CreatedResponse(string? uri, object? result = null)
    {
        if (!NotificationContext.HasNotifications)
        {
            return Created(uri ?? string.Empty, new ApiResponse(success: true, data: result));
        }

        return Response(result);
    }
}