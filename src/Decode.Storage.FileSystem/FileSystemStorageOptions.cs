namespace Decode.Storage.FileSystem;

/// <summary>
/// Configuration options for local file system storage.
/// </summary>
public class FileSystemStorageOptions
{
    /// <summary>
    /// The root directory path where files will be stored.
    /// </summary>
    public string BasePath { get; set; } = string.Empty;
}
