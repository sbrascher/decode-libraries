# Decode.Security.Jwt

A high-performance and reusable JWT (JSON Web Token) library for the Decode ecosystem.

## 🚀 Features

- **Decoupled:** Completely independent of business logic (works with `Claims`).
- **High Performance:** Uses `Microsoft.IdentityModel.JsonWebTokens` (the newest and fastest JWT library for .NET).
- **Modern:** Built for .NET 8/9, targeting `netstandard2.1` for maximum compatibility.
- **Full Support:** Includes token generation AND validation.

## 📦 Installation

Register the service in your `Program.cs`:

```csharp
using Decode.Security.Jwt.Extensions;

builder.Services.AddJwt(options =>
{
    options.Secret = builder.Configuration["Jwt:Secret"];
    options.Issuer = "MyApplication";
    options.Audience = "MyApplicationUsers";
    options.DefaultExpiresMinutes = 60;
});
```

## 📖 Usage

Inject `IJwtService` into your controllers or services:

```csharp
public class AuthService
{
    private readonly IJwtService _jwtService;

    public AuthService(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public string Login(User user)
    {
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        return _jwtService.CreateToken(claims);
    }
}
```

### Validating a Token

To validate a token and retrieve its claims, use `GetPrincipalFromToken` or its asynchronous equivalent `GetPrincipalFromTokenAsync`:

```csharp
using System.Security.Claims;

public class TokenValidatorService
{
    private readonly IJwtService _jwtService;

    public TokenValidatorService(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<bool> ValidateUserToken(string token)
    {
        // Validates signature, issuer, audience, and expiration lifetime.
        // Returns a ClaimsPrincipal containing the claims if valid; otherwise, returns null.
        ClaimsPrincipal? principal = await _jwtService.GetPrincipalFromTokenAsync(token);
        
        if (principal == null)
        {
            return false;
        }

        var userId = principal.FindFirst("id")?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        return true;
    }
}
```

## 📄 License
MIT License.
