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

### IFileValidator & FileSignatureValidator
Provides secure, low-overhead file type validation by inspecting file magic numbers (signatures) instead of relying solely on the file extension or the client-supplied content type. It incorporates a blocklist of dangerous executable headers (such as Windows PE/MZ, Linux ELF, Java Bytecode, and shell shebangs) to prevent MIME-spoofing and script execution attacks.

```csharp
public interface IFileValidator
{
    Task<bool> IsValidAsync(Stream stream, string fileName, IEnumerable<string> allowedExtensions, CancellationToken cancellationToken = default);
}
```

#### Example Usage

```csharp
public class UploadService
{
    private readonly IFileValidator _fileValidator;
    private readonly IStorageService _storageService;

    public UploadService(IFileValidator fileValidator, IStorageService storageService)
    {
        _fileValidator = fileValidator;
        _storageService = storageService;
    }

    public async Task SaveAvatarAsync(string fileName, Stream fileStream)
    {
        string[] allowedExtensions = [".png", ".jpg", ".jpeg"];

        // Validates signatures and ensures the file is not an executable, even if renamed to .png
        if (!await _fileValidator.IsValidAsync(fileStream, fileName, allowedExtensions))
        {
            throw new SecurityException("Malicious or unsupported file type detected.");
        }

        // Proceed to upload securely
        await _storageService.UploadAsync($"avatars/{fileName}", fileStream);
    }
}
```

## 📖 Why Abstractions?

By depending on `IStorageService`, your application services and business domain remain completely decoupled from specific cloud storage SDKs or physical disk access patterns, enabling seamless switching between Local FileSystem for development/testing and Cloud Storage for production.

## 📄 License
MIT License.
