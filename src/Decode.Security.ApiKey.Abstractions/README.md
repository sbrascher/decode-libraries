# Decode.Security.ApiKey.Abstractions

Core abstractions and options for API Key authentication within the Decode ecosystem.

## 📦 Installation

```bash
dotnet add package Decode.Security.ApiKey.Abstractions
```

## 🛠️ Key Components

### IApiKeyValidator
The contract that must be implemented to validate incoming API Keys.

```csharp
public interface IApiKeyValidator
{
    Task<ApiKeyValidationResult> ValidateAsync(string apiKey, CancellationToken cancellationToken = default);
}
```

### ApiKeyValidationResult
The result type returned by the validator, encapsulating success state and the associated `ClaimsPrincipal`.

```csharp
// To return success:
return ApiKeyValidationResult.Success(claimsPrincipal);

// To return failure:
return ApiKeyValidationResult.Failure();
```

## 📄 License
MIT License.
