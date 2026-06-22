namespace Decode.Storage.Abstractions;

/// <summary>
/// Represents a file retrieved from the storage provider, including its content stream and metadata.
/// </summary>
/// <param name="Content">The readable stream containing the file data.</param>
/// <param name="ContentType">The MIME content type of the file (e.g., "image/png").</param>
/// <param name="FileName">The name of the file.</param>
/// <param name="Length">The size of the file in bytes.</param>
public record StorageFile(
    Stream Content,
    string ContentType,
    string FileName,
    long Length) : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Disposes the underlying content stream.
    /// </summary>
    public void Dispose()
    {
        Content.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the underlying content stream asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await Content.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
