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
