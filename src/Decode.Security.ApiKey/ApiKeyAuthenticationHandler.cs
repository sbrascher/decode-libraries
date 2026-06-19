using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Decode.Security.ApiKey;

/// <summary>
/// Authentication handler for processing API Key authentication.
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyOptions>
{
    private readonly IApiKeyValidator _apiKeyValidator;

    /// <summary>
    /// Initializes a new instance of <see cref="ApiKeyAuthenticationHandler"/>.
    /// </summary>
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyValidator apiKeyValidator)
        : base(options, logger, encoder)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    /// <summary>
    /// Handles the API Key authentication process.
    /// </summary>
    /// <returns>The <see cref="AuthenticateResult"/> of the authentication process.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyValues))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = apiKeyValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return AuthenticateResult.Fail("API Key is empty.");
        }

        try
        {
            var result = await _apiKeyValidator.ValidateAsync(apiKey, Context.RequestAborted);
            if (!result.IsValid || result.Principal == null)
            {
                Logger.LogWarning("API Key validation failed.");
                return AuthenticateResult.Fail("Invalid API Key.");
            }

            var ticket = new AuthenticationTicket(result.Principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred during API Key validation.");
            return AuthenticateResult.Fail(ex);
        }
    }
}
