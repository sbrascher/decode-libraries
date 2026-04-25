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
    }

    /// <inheritdoc />
    public void Add(DomainNotification notification)
    {
        _notifications.Add(notification);
    }

    /// <inheritdoc />
    public void AddRange(IEnumerable<DomainNotification> notifications)
    {
        _notifications.AddRange(notifications);
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