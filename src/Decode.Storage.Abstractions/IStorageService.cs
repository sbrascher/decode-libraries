namespace Decode.Storage.Abstractions;

/// <summary>
/// Defines the contract for cloud and local storage operations.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Uploads a file stream to the specified path or unique key.
    /// </summary>
    /// <param name="path">The relative path or key where the file will be stored.</param>
    /// <param name="content">The stream containing the file content to upload.</param>
    /// <param name="contentType">The MIME type of the file (e.g. "application/pdf").</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A string representing the path, key, or URI of the uploaded file.</returns>
    Task<string> UploadAsync(
        string path,
        Stream content,
        string? contentType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file from the specified path or unique key.
    /// </summary>
    /// <param name="path">The path or key of the file to download.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="StorageFile"/> instance containing the stream and metadata, or null if the file does not exist.</returns>
    Task<StorageFile?> DownloadAsync(
        string path,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a file exists at the specified path or unique key.
    /// </summary>
    /// <param name="path">The path or key to check.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>True if the file exists; otherwise false.</returns>
    Task<bool> ExistsAsync(
        string path,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from the storage.
    /// </summary>
    /// <param name="path">The path or key of the file to delete.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>True if the file was deleted; false if the file was not found or could not be deleted.</returns>
    Task<bool> DeleteAsync(
        string path,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a public URL or a secure temporary (pre-signed) URL for the file.
    /// </summary>
    /// <param name="path">The path or key of the file.</param>
    /// <param name="expiration">Optional expiration date and time for temporary URLs. If null, the provider's default or a public URL is returned.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A string containing the direct access URL to the file.</returns>
    Task<string> GetUrlAsync(
        string path,
        DateTimeOffset? expiration = null,
        CancellationToken cancellationToken = default);
}
