# Decode.Telemetry

OpenTelemetry instrumentation and extension methods for the Decode library ecosystem.

## 🚀 Features

- **Decoupled & Standardized:** Built on top of the native OpenTelemetry SDK for .NET.
- **Unified Registration:** Register tracing and metrics for the entire Decode ecosystem in a single call.
- **Trace Support:** Spans generated for `Decode.Data` database connections and transactions.
- **Metrics Support:** Performance counters for `Decode.Security.Jwt` token operations and `Decode.Notifications` business alerts.

## 📦 Installation

```bash
dotnet add package Decode.Telemetry
```

## 🛠️ Setup

Register the instrumentation inside your `Program.cs` OpenTelemetry builder setup:

```csharp
using Decode.Telemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddDecodeInstrumentation() // Registers Decode.Data and Decode.Security.Jwt trace sources
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddDecodeInstrumentation() // Registers Decode.Security.Jwt and Decode.Notifications meters
        .AddPrometheusExporter());
```

## 📊 Telemetry Data Details

### Traces

The library automatically captures:
- `DbSession.CreateConnection`: Open operations on database connections.
- `UnitOfWork.BeginTransaction`: Transaction startup durations.
- `UnitOfWork.Commit`: Transaction commits.
- `UnitOfWork.Rollback`: Transaction rollbacks.
- `JwtService.CreateToken`: JWT generation times.
- `JwtService.ValidateToken`: JWT validation times and internal validation status.

### Metrics

The library exposes the following standard .NET Counters:
- `decode.jwt.tokens.generated`: Count of successfully generated tokens.
- `decode.jwt.tokens.validated`: Count of successfully validated tokens.
- `decode.jwt.tokens.validation_failures`: Count of token validation failures (includes a `reason` tag describing why, e.g., expired or invalid signature).
- `decode.notifications.added`: Count of Domain Notifications added in business logic (includes a `key` tag representing the field name and `status_code` for the HTTP response type).

## 📄 License
MIT License.
