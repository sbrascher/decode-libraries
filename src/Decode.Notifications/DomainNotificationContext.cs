using System.Net;

namespace Decode.Notifications;

/// <summary>
/// Implementation of the domain notification context.
/// </summary>
public class DomainNotificationContext : IDomainNotificationContext
{
    private readonly List<DomainNotification> _notifications = [];

    /// <inheritdoc />
    public IReadOnlyCollection<DomainNotification> Notifications => _notifications.AsReadOnly();

    /// <inheritdoc />
    public bool HasNotifications => _notifications.Count > 0;

    /// <inheritdoc />
    public void Add(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? key = null)
    {
        _notifications.Add(new DomainNotification(message, (int)statusCode, key));
        DecodeNotificationsDiagnostics.NotificationAddedCounter.Add(1, 
            new KeyValuePair<string, object?>("key", key ?? "global"),
            new KeyValuePair<string, object?>("status_code", (int)statusCode));
    }

    /// <inheritdoc />
    public void Add(DomainNotification notification)
    {
        _notifications.Add(notification);
        DecodeNotificationsDiagnostics.NotificationAddedCounter.Add(1, 
            new KeyValuePair<string, object?>("key", notification.Key ?? "global"),
            new KeyValuePair<string, object?>("status_code", notification.StatusCode));
    }

    /// <inheritdoc />
    public void AddRange(IEnumerable<DomainNotification> notifications)
    {
        foreach (var notification in notifications)
        {
            Add(notification);
        }
    }

    /// <inheritdoc />
    public int GetStatusCode()
    {
        return HasNotifications
            ? _notifications.Max(n => n.StatusCode)
            : (int)HttpStatusCode.OK;
    }

    /// <inheritdoc />
    public IEnumerable<string> GetMessages()
    {
        return _notifications.Select(n => n.Message);
    }
}