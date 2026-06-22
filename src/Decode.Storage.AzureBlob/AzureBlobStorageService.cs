using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Decode.Storage.Abstractions;
using Microsoft.Extensions.Options;

namespace Decode.Storage.AzureBlob;

/// <summary>
/// Implementation of <see cref="IStorageService"/> for Azure Blob Storage.
/// </summary>
public class AzureBlobStorageService(IOptions<AzureBlobStorageOptions> options) : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient = new(options.Value.ConnectionString);
    private readonly string _containerName = options.Value.ContainerName;
    private readonly bool _createContainerIfNotExists = options.Value.CreateContainerIfNotExists;

    /// <inheritdoc />
    public async Task<string> UploadAsync(
        string path,
        Stream content,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        if (_createContainerIfNotExists)
        {
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }

        BlobClient blobClient = containerClient.GetBlobClient(path);

        BlobUploadOptions uploadOptions = new();
        if (!string.IsNullOrEmpty(contentType))
        {
            uploadOptions.HttpHeaders = new BlobHttpHeaders { ContentType = contentType };
        }

        await blobClient.UploadAsync(content, uploadOptions, cancellationToken);
        return blobClient.Uri.AbsoluteUri;
    }

    /// <inheritdoc />
    public async Task<StorageFile?> DownloadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(path);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return null;
        }

        // Using streaming to avoid buffering files in memory
        Azure.Response<BlobDownloadStreamingResult> response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        BlobDownloadStreamingResult result = response.Value;

        string contentType = result.Details.ContentType;
        string fileName = Path.GetFileName(path);
        long length = result.Details.ContentLength;

        return new StorageFile(result.Content, contentType, fileName, length);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(path);

        return await blobClient.ExistsAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(path);

        Azure.Response<bool> response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        return response.Value;
    }

    /// <inheritdoc />
    public Task<string> GetUrlAsync(
        string path,
        DateTimeOffset? expiration = null,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(path);

        if (expiration.HasValue)
        {
            if (!blobClient.CanGenerateSasUri)
            {
                throw new InvalidOperationException("The BlobClient cannot generate a SAS URI. Check account keys or authentication configurations.");
            }

            BlobSasBuilder sasBuilder = new(BlobContainerSasPermissions.Read, expiration.Value)
            {
                BlobContainerName = _containerName,
                BlobName = path
            };

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return Task.FromResult(sasUri.AbsoluteUri);
        }

        return Task.FromResult(blobClient.Uri.AbsoluteUri);
    }
}
