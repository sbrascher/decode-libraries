using System.Security.Claims;

namespace Decode.Security.Jwt;

/// <summary>
/// Configuration options for JWT generation and validation.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The secret key used to sign the tokens.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// The issuer of the token.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The audience of the token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Default expiration time in minutes for access tokens.
    /// </summary>
    public int DefaultExpiresMinutes { get; set; } = 60;
}

/// <summary>
/// Service for handling JWT (JSON Web Token) operations.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Creates a JWT token with the specified claims.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <param name="expiresMinutes">Optional override for the expiration time.</param>
    /// <returns>A string representation of the JWT.</returns>
    string CreateToken(IEnumerable<Claim> claims, int? expiresMinutes = null);

    /// <summary>
    /// Validates a JWT token and returns the principal if valid.
    /// </summary>
    /// <param name="token">The JWT token string.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> if the token is valid; otherwise, null.</returns>
    ClaimsPrincipal? GetPrincipalFromToken(string token);

    /// <summary>
    /// Asynchronously validates a JWT token and returns the principal if valid.
    /// </summary>
    /// <param name="token">The JWT token string.</param>
    /// <returns>A task that represents the asynchronous operation, containing a <see cref="ClaimsPrincipal"/> if valid.</returns>
    Task<ClaimsPrincipal?> GetPrincipalFromTokenAsync(string token);
}
