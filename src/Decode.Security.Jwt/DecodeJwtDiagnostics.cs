using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Decode.Security.Jwt;

internal static class DecodeJwtDiagnostics
{
    public static readonly ActivitySource Source = new("Decode.Security.Jwt");
    public static readonly Meter Meter = new("Decode.Security.Jwt");

    public static readonly Counter<long> TokenGeneratedCounter = Meter.CreateCounter<long>(
        "decode.jwt.tokens.generated",
        description: "Number of JWT tokens generated successfully");

    public static readonly Counter<long> TokenValidatedCounter = Meter.CreateCounter<long>(
        "decode.jwt.tokens.validated",
        description: "Number of JWT tokens validated successfully");

    public static readonly Counter<long> TokenValidationErrorCounter = Meter.CreateCounter<long>(
        "decode.jwt.tokens.validation_failures",
        description: "Number of JWT token validation failures");
}
