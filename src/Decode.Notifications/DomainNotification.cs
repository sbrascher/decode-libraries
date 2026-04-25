using System.Net;

namespace Decode.Notifications;

/// <summary>
/// Represents a domain notification.
/// </summary>
public class DomainNotification
{
    /// <summary>
    /// Gets the notification message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the HTTP status code associated with the notification (default is 400 Bad Request).
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Gets an optional key or identifier for the notification (e.g., field name).
    /// </summary>
    public string? Key { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainNotification"/> class.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="key">The optional key.</param>
    public DomainNotification(string message, int statusCode = (int)HttpStatusCode.BadRequest, string? key = null)
    {
        Message = message;
        StatusCode = statusCode;
        Key = key;
    }
}