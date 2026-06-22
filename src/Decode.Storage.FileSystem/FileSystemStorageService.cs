using Decode.Storage.Abstractions;
using Microsoft.Extensions.Options;

namespace Decode.Storage.FileSystem;

/// <summary>
/// Implementation of <see cref="IStorageService"/> that saves and retrieves files from the local file system.
/// </summary>
public class FileSystemStorageService(IOptions<FileSystemStorageOptions> options) : IStorageService
{
    private readonly string _basePath = options.Value.BasePath;

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

        if (string.IsNullOrWhiteSpace(_basePath))
        {
            throw new InvalidOperationException("The base storage path has not been configured.");
        }

        string fullPath = GetFullPath(path);
        string? directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // To avoid locking issues and half-written files in case of cancellation or crash,
        // we write to a temporary file first and then perform an atomic rename/move operation.
        string tempFilePath = Path.Combine(directory ?? _basePath, $".tmp_{Guid.NewGuid():N}");

        try
        {
            // Configure FileStream for asynchronous I/O and permit concurrent reads.
            const int bufferSize = 4096;
            using (FileStream tempStream = new(
                tempFilePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read,
                bufferSize,
                useAsync: true))
            {
                await content.CopyToAsync(tempStream, bufferSize, cancellationToken);
                await tempStream.FlushAsync(cancellationToken);
            }

            // Move the file atomically to its final location, overwriting if it already exists.
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            File.Move(tempFilePath, fullPath);
        }
        catch
        {
            // Cleanup the temporary file if something went wrong
            if (File.Exists(tempFilePath))
            {
                try
                {
                    File.Delete(tempFilePath);
                }
                catch
                {
                    // Ignore cleanup failures to not mask the original exception
                }
            }
            throw;
        }

        return fullPath;
    }

    /// <inheritdoc />
    public Task<StorageFile?> DownloadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(_basePath))
        {
            throw new InvalidOperationException("The base storage path has not been configured.");
        }

        string fullPath = GetFullPath(path);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<StorageFile?>(null);
        }

        // Open with FileShare.Read to prevent write-locks while reading.
        // The stream is returned and will be disposed by the caller (implementing IDisposable / IAsyncDisposable).
        const int bufferSize = 4096;
        FileStream fileStream = new(
            fullPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            useAsync: true);

        string contentType = GetContentType(fullPath);
        string fileName = Path.GetFileName(fullPath);
        long length = fileStream.Length;

        StorageFile file = new(fileStream, contentType, fileName, length);
        return Task.FromResult<StorageFile?>(file);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(_basePath))
        {
            throw new InvalidOperationException("The base storage path has not been configured.");
        }

        string fullPath = GetFullPath(path);
        bool exists = File.Exists(fullPath);
        return Task.FromResult(exists);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(_basePath))
        {
            throw new InvalidOperationException("The base storage path has not been configured.");
        }

        string fullPath = GetFullPath(path);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult(false);
        }

        File.Delete(fullPath);
        return Task.FromResult(true);
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

        if (string.IsNullOrWhiteSpace(_basePath))
        {
            throw new InvalidOperationException("The base storage path has not been configured.");
        }

        string fullPath = GetFullPath(path);
        Uri fileUri = new(fullPath);
        string url = fileUri.AbsoluteUri;
        return Task.FromResult(url);
    }

    private string GetFullPath(string relativePath)
    {
        // Sanitize the relative path to prevent Directory Traversal attacks (e.g. "..\..\etc")
        string cleanPath = relativePath.Replace("..", "").TrimStart('/', '\\');
        return Path.GetFullPath(Path.Combine(_basePath, cleanPath));
    }

    private static string GetContentType(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();
        return extension switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".html" => "text/html",
            ".csv" => "text/csv",
            _ => "application/octet-stream"
        };
    }
}
