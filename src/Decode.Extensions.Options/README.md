# Decode.Extensions.Options

Utility extensions for the **Options Pattern** in .NET, ensuring that configurations are not only registered but also present and valid at runtime.

## 📦 Installation

```bash
dotnet add package Decode.Extensions.Options
```

## 🛠️ Usage

### 1. Registering Options with Mandatory Section

The `AddValidatedOptions<T>` method ensures that the section exists in your `appsettings.json`. By default, it looks for a section with the same name as the class `T`.

```csharp
using Decode.Extensions.Options;

// In Program.cs
builder.Services.AddValidatedOptions<DatabaseSettings>(builder.Configuration);
```

### 2. Immediate Configuration Access

Retrieve a configuration value directly from `IConfiguration` with immediate validation.

```csharp
var settings = builder.Configuration.GetValidatedSectionValue<MySettings>();
```

### 3. Benefit

Avoid "Silent Failures" where your application starts but crashes later because an `IOptions<T>` was injected with empty values due to a missing section in the configuration provider.

## 📄 License
MIT License.
