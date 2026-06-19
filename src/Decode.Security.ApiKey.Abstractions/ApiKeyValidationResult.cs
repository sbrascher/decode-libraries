using System.Security.Claims;

namespace Decode.Security.ApiKey;

/// <summary>
/// Represents the result of validating an API key.
/// </summary>
public class ApiKeyValidationResult
{
    /// <summary>
    /// Gets a value indicating whether validation succeeded.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the claims principal associated with the API key owner (if valid).
    /// </summary>
    public ClaimsPrincipal? Principal { get; }

    private ApiKeyValidationResult(bool isValid, ClaimsPrincipal? principal)
    {
        IsValid = isValid;
        Principal = principal;
    }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    /// <param name="principal">The principal associated with the key.</param>
    /// <returns>A successful <see cref="ApiKeyValidationResult"/>.</returns>
    public static ApiKeyValidationResult Success(ClaimsPrincipal principal) => new(true, principal);

    /// <summary>
    /// Creates a failed validation result.
    /// </summary>
    /// <returns>A failed <see cref="ApiKeyValidationResult"/>.</returns>
    public static ApiKeyValidationResult Failure() => new(false, null);
}
