using System.Net;

namespace Decode.Notifications;

/// <summary>
/// Interface for the domain notification context.
/// </summary>
public interface IDomainNotificationContext
{
    /// <summary>
    /// Gets the list of notifications.
    /// </summary>
    IReadOnlyCollection<DomainNotification> Notifications { get; }

    /// <summary>
    /// Gets a value indicating whether there are any notifications.
    /// </summary>
    bool HasNotifications { get; }

    /// <summary>
    /// Adds a new notification.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="statusCode">The HTTP status code associated with the notification.</param>
    /// <param name="key">An optional key to identify the notification.</param>
    void Add(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? key = null);

    /// <summary>
    /// Adds a notification object.
    /// </summary>
    /// <param name="notification">The notification object.</param>
    void Add(DomainNotification notification);

    /// <summary>
    /// Adds a range of notifications.
    /// </summary>
    /// <param name="notifications">The collection of notifications to add.</param>
    void AddRange(IEnumerable<DomainNotification> notifications);

    /// <summary>
    /// Gets the highest status code from the notifications, or 200 (OK) if there are none.
    /// </summary>
    /// <returns>The HTTP status code.</returns>
    int GetStatusCode();

    /// <summary>
    /// Gets all notification messages.
    /// </summary>
    /// <returns>A collection of messages.</returns>
    IEnumerable<string> GetMessages();
}
