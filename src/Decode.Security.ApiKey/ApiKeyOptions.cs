using Microsoft.AspNetCore.Authentication;

namespace Decode.Security.ApiKey;

/// <summary>
/// Configuration options for API Key authentication.
/// </summary>
public class ApiKeyOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The HTTP Header name that holds the API Key. Default is "X-Api-Key".
    /// </summary>
    public string HeaderName { get; set; } = "X-Api-Key";

    /// <summary>
    /// The Authentication Scheme name. Default is "ApiKey".
    /// </summary>
    public string Scheme { get; set; } = "ApiKey";
}
