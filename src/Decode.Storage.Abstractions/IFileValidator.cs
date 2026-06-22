namespace Decode.Storage.Abstractions;

/// <summary>
/// Defines a contract for validating files by inspecting their content signatures (Magic Numbers) and extensions.
/// </summary>
public interface IFileValidator
{
    /// <summary>
    /// Validates if the file content signature matches the allowed extensions and ensures the file is not a known executable.
    /// </summary>
    /// <param name="stream">The file content stream. Must be readable and support seeking to allow resetting position.</param>
    /// <param name="fileName">The name of the file (including its extension).</param>
    /// <param name="allowedExtensions">The list of permitted extensions (e.g., ".png", ".pdf", ".jpg"). Extension checking is case-insensitive.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>True if the file matches one of the allowed extensions' signatures and is secure; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the stream does not support seeking.</exception>
    Task<bool> IsValidAsync(
        Stream stream,
        string fileName,
        IEnumerable<string> allowedExtensions,
        CancellationToken cancellationToken = default);
}
