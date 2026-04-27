# Decode.Security.Jwt.Abstractions

Core abstractions and options for the **Decode.Security.Jwt** ecosystem, providing contracts for token generation and validation.

## 📦 Installation

```bash
dotnet add package Decode.Security.Jwt.Abstractions
```

## 🛠️ Key Components

### IJwtService
The main interface for JWT operations.

```csharp
public interface IJwtService
{
    string CreateToken(IEnumerable<Claim> claims, int? expiresMinutes = null);
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    Task<ClaimsPrincipal?> GetPrincipalFromTokenAsync(string token);
}
```

### JwtOptions
Configuration class for JWT settings.

```csharp
public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int DefaultExpiresMinutes { get; set; } = 60;
}
```

## 📖 Why Abstractions?

Using these abstractions allows your application to remain decoupled from the specific JWT implementation (e.g., `Microsoft.IdentityModel.JsonWebTokens`), making it easier to test and maintain.

## 📄 License
MIT License.
