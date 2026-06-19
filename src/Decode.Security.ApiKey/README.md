# Decode.Security.ApiKey

Implementation of API Key authentication handler and service extensions for ASP.NET Core in the Decode ecosystem.

## 🚀 Features

- **Standard Integration:** Plugs directly into ASP.NET Core's native Authentication middleware.
- **Custom Validation:** Easily delegate validation logic (database, appsettings, etc.) by implementing a simple interface.
- **Flexible Claims:** Bind client information or roles to the claims principal associated with the key.

## 📦 Installation

```bash
dotnet add package Decode.Security.ApiKey
```

## 🛠️ Setup

### 1. Implement your Validator

Implement `IApiKeyValidator` to define how keys are validated (e.g., retrieving from a database or configuration):

```csharp
using System.Security.Claims;
using Decode.Security.ApiKey;

public class MyApiKeyValidator : IApiKeyValidator
{
    private readonly IConfiguration _configuration;

    public MyApiKeyValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<ApiKeyValidationResult> ValidateAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        var expectedKey = _configuration["ApiKey"];

        if (apiKey == expectedKey)
        {
            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "ThirdPartyIntegration"),
                new Claim(ClaimTypes.Role, "ServiceClient")
            };
            var identity = new ClaimsIdentity(claims, "ApiKey");
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(ApiKeyValidationResult.Success(principal));
        }

        return Task.FromResult(ApiKeyValidationResult.Failure());
    }
}
```

### 2. Register Services

In your `Program.cs`, add API Key authentication and register your custom validator:

```csharp
using Decode.Security.ApiKey.Extensions;

// Register API Key authentication using your custom validator
builder.Services.AddApiKey<MyApiKeyValidator>(options =>
{
    options.HeaderName = "X-Api-Key"; // Custom header name (default is "X-Api-Key")
    options.Scheme = "ApiKey";         // Custom scheme name (default is "ApiKey")
});

builder.Services.AddAuthorization();
```

### 3. Configure Middleware

Ensure authentication and authorization middlewares are in your pipeline:

```csharp
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

## 📖 Usage

### Securing Controllers or Actions

Use the `[Authorize]` attribute with your custom scheme name:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "ApiKey")]
public class IntegrationController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        var clientName = User.Identity?.Name;
        return Ok(new { message = $"Hello, {clientName}. You have access!" });
    }
}
```

## 📄 License
MIT License.
