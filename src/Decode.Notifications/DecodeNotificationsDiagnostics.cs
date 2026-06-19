using System.Diagnostics.Metrics;

namespace Decode.Notifications;

internal static class DecodeNotificationsDiagnostics
{
    public static readonly Meter Meter = new("Decode.Notifications");

    public static readonly Counter<long> NotificationAddedCounter = Meter.CreateCounter<long>(
        "decode.notifications.added",
        description: "Number of domain notifications added");
}
