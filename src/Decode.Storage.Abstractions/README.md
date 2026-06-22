# Decode.Storage.Abstractions

Core abstractions and interfaces for the **Decode.Storage** ecosystem, providing contracts for file management in .NET.

## 📦 Installation

```bash
dotnet add package Decode.Storage.Abstractions
```

## 🛠️ Components

### IStorageService
Defines the contract for storing, retrieving, checking, and deleting files agnostic to the underlying storage provider (Local FileSystem, Azure Blob Storage, AWS S3, etc.).

```csharp
public interface IStorageService
{
    Task<string> UploadAsync(string path, Stream content, string? contentType = null, CancellationToken cancellationToken = default);
    Task<StorageFile?> DownloadAsync(string path, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default);
    Task<string> GetUrlAsync(string path, DateTimeOffset? expiration = null, CancellationToken cancellationToken = default);
}
```

### StorageFile
A lightweight record wrapping the file stream and basic metadata. It implements `IDisposable` and `IAsyncDisposable` to ensure the file stream is properly disposed of after consumption.

```csharp
public record StorageFile(Stream Content, string ContentType, string FileName, long Length) : IDisposable, IAsyncDisposable;
```

## 📖 Why Abstractions?

By depending on `IStorageService`, your application services and business domain remain completely decoupled from specific cloud storage SDKs or physical disk access patterns, enabling seamless switching between Local FileSystem for development/testing and Cloud Storage for production.

## 📄 License
MIT License.
