# Decode Libraries

An ecosystem of modern, lightweight, and high-performance .NET libraries designed to accelerate application development.

## 🚀 Available Libraries

| Library | Version | Description |
| :--- | :--- | :--- |
| **Decode.AspNetCore** | ![NuGet](https://img.shields.io/nuget/v/Decode.AspNetCore) | Standardized infrastructure for APIs, global exception middleware, and base controller. |
| **Decode.Cryptography** | ![NuGet](https://img.shields.io/nuget/v/Decode.Cryptography) | Modern secure cryptography utilities (PBKDF2, HMAC-SHA256, constant-time checks). |
| **Decode.Data** | ![NuGet](https://img.shields.io/nuget/v/Decode.Data) | Agnostic concrete implementation of Unit of Work and DbSession. |
| **Decode.Data.Abstractions** | ![NuGet](https://img.shields.io/nuget/v/Decode.Data.Abstractions) | Core contracts and interfaces for connection and transaction management. |
| **Decode.Extensions** | ![NuGet](https://img.shields.io/nuget/v/Decode.Extensions) | Utility extensions for .NET basic types and enum parsing. |
| **Decode.Extensions.Options** | ![NuGet](https://img.shields.io/nuget/v/Decode.Extensions.Options) | Prevent silent startup failures by validating Option configurations. |
| **Decode.Notifications** | ![NuGet](https://img.shields.io/nuget/v/Decode.Notifications) | Domain Notification pattern context to capture validations without exceptions. |
| **Decode.Notifications.FluentValidation** | ![NuGet](https://img.shields.io/nuget/v/Decode.Notifications.FluentValidation) | Integrates FluentValidation results into the domain notification context. |
| **Decode.Security.ApiKey** | ![NuGet](https://img.shields.io/nuget/v/Decode.Security.ApiKey) | Native ASP.NET Core API Key authentication middleware integration. |
| **Decode.Security.ApiKey.Abstractions** | ![NuGet](https://img.shields.io/nuget/v/Decode.Security.ApiKey.Abstractions) | Abstractions and validators for API Key security. |
| **Decode.Security.Jwt** | ![NuGet](https://img.shields.io/nuget/v/Decode.Security.Jwt) | High-performance JWT service implementation using modern IdentityModel. |
| **Decode.Security.Jwt.Abstractions** | ![NuGet](https://img.shields.io/nuget/v/Decode.Security.Jwt.Abstractions) | Abstractions and configurations for JWT authentication. |
| **Decode.Storage.Abstractions** | ![NuGet](https://img.shields.io/nuget/v/Decode.Storage.Abstractions) | Agnostic contracts and models for file storage operations. |
| **Decode.Storage.FileSystem** | ![NuGet](https://img.shields.io/nuget/v/Decode.Storage.FileSystem) | Local FileSystem storage implementation with thread-safe and atomic operations. |
| **Decode.Storage.AzureBlob** | ![NuGet](https://img.shields.io/nuget/v/Decode.Storage.AzureBlob) | Cloud Azure Blob Storage implementation using the Azure SDK. |
| **Decode.Telemetry** | ![NuGet](https://img.shields.io/nuget/v/Decode.Telemetry) | OpenTelemetry metrics and tracing registration for the entire Decode ecosystem. |

## 🛠️ Technologies
- .NET Standard 2.1+
- .NET 8 / .NET 9 / .NET 10
- C# 12

## 📦 Building from Source
To build all libraries in the repository:
```bash
dotnet build src/Decode.sln
```

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
