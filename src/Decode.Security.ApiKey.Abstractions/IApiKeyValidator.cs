namespace Decode.Security.ApiKey;

/// <summary>
/// Defines the contract for validating API keys.
/// </summary>
public interface IApiKeyValidator
{
    /// <summary>
    /// Asynchronously validates an API key and returns the validation result.
    /// </summary>
    /// <param name="apiKey">The API key to validate.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the operation, returning the <see cref="ApiKeyValidationResult"/>.</returns>
    Task<ApiKeyValidationResult> ValidateAsync(string apiKey, CancellationToken cancellationToken = default);
}
