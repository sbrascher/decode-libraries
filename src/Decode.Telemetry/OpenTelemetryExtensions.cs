using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Decode.Telemetry;

/// <summary>
/// Extension methods for registering Decode ecosystem telemetry.
/// </summary>
public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Adds tracing sources for all Decode libraries to the OpenTelemetry <see cref="TracerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder">The tracer provider builder.</param>
    /// <returns>The updated builder.</returns>
    public static TracerProviderBuilder AddDecodeInstrumentation(this TracerProviderBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder
            .AddSource("Decode.Data")
            .AddSource("Decode.Security.Jwt");
    }

    /// <summary>
    /// Adds metrics sources for all Decode libraries to the OpenTelemetry <see cref="MeterProviderBuilder"/>.
    /// </summary>
    /// <param name="builder">The meter provider builder.</param>
    /// <returns>The updated builder.</returns>
    public static MeterProviderBuilder AddDecodeInstrumentation(this MeterProviderBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder
            .AddMeter("Decode.Security.Jwt")
            .AddMeter("Decode.Notifications");
    }
}
