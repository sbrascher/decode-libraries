# Decode.Storage.FileSystem

Local FileSystem storage implementation of `IStorageService` for the **Decode.Storage** ecosystem.

## 📦 Installation

```bash
dotnet add package Decode.Storage.FileSystem
```

## 🛠️ Usage

### 1. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
using Decode.Storage.FileSystem.Extensions;

builder.Services.AddFileSystemStorage(options =>
{
    options.BasePath = "C:\\StorageRoot"; // Or load from configuration
});
```

### 2. Inject and Use in Services

```csharp
using Decode.Storage.Abstractions;

public class DocumentService
{
    private readonly IStorageService _storage;

    public DocumentService(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task SaveReportAsync(string fileName, Stream content)
    {
        // Thread-safe writing using file-sharing flags and atomic file moves internally.
        string savedPath = await _storage.UploadAsync($"reports/{fileName}", content, "application/pdf");
    }

    public async Task<StorageFile?> GetReportAsync(string fileName)
    {
        // Safe streaming and reading
        return await _storage.DownloadAsync($"reports/{fileName}");
    }
}
```

## 📄 License
MIT License.
