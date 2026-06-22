# Decode.Storage.AzureBlob

Azure Blob Storage implementation of `IStorageService` for the **Decode.Storage** ecosystem.

## 📦 Installation

```bash
dotnet add package Decode.Storage.AzureBlob
```

## 🛠️ Usage

### 1. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
using Decode.Storage.AzureBlob.Extensions;

builder.Services.AddAzureBlobStorage(options =>
{
    options.ConnectionString = "UseDevelopmentStorage=true"; // Azure Storage Account connection string
    options.ContainerName = "my-container";                  // Target container
    options.CreateContainerIfNotExists = true;               // Automatically create container on uploads if missing
});
```

> [!NOTE]
> This registration automatically registers `IFileValidator` (implemented by `FileSignatureValidator`) as a singleton in your dependency injection container.

### 2. Inject and Use in Services

```csharp
using Decode.Storage.Abstractions;

public class ProfileService
{
    private readonly IStorageService _storage;

    public ProfileService(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task SaveAvatarAsync(Guid userId, Stream content)
    {
        // Uploads the stream directly to Azure Blob container, setting the appropriate Content-Type header.
        string blobUrl = await _storage.UploadAsync($"avatars/{userId}.png", content, "image/png");
    }

    public async Task<StorageFile?> GetAvatarAsync(Guid userId)
    {
        // Downloads the file as a non-buffering stream directly from Azure Blob Storage.
        return await _storage.DownloadAsync($"avatars/{userId}.png");
    }

    public async Task<string> GetTemporaryDownloadUrlAsync(Guid userId)
    {
        // Generates a secure pre-signed SAS URI that expires in 15 minutes.
        return await _storage.GetUrlAsync($"avatars/{userId}.png", DateTimeOffset.UtcNow.AddMinutes(15));
    }
}
```

## 📄 License
MIT License.
