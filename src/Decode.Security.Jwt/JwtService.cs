using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Decode.Security.Jwt;

/// <summary>
/// Default implementation of <see cref="IJwtService"/> using <see cref="JsonWebTokenHandler"/>.
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtOptions _options;
    private readonly JsonWebTokenHandler _tokenHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="options">The JWT configuration options.</param>
    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _tokenHandler = new JsonWebTokenHandler();
    }

    /// <inheritdoc />
    public string CreateToken(IEnumerable<Claim> claims, int? expiresMinutes = null)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_options.Secret));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiresMinutes ?? _options.DefaultExpiresMinutes),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = credentials
        };

        return _tokenHandler.CreateToken(tokenDescriptor);
    }

    /// <inheritdoc />
    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        // For simple, compute-bound synchronous scenarios, we can call the async method 
        // without risking deadlock in ASP.NET Core due to lack of SynchronizationContext.
        return GetPrincipalFromTokenAsync(token).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task<ClaimsPrincipal?> GetPrincipalFromTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            ValidateIssuer = !string.IsNullOrEmpty(_options.Issuer),
            ValidIssuer = _options.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(_options.Audience),
            ValidAudience = _options.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        TokenValidationResult result = await _tokenHandler.ValidateTokenAsync(token, validationParameters);

        return result.IsValid ? new ClaimsPrincipal(result.ClaimsIdentity) : null;
    }
}