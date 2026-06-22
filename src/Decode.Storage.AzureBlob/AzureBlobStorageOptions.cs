namespace Decode.Storage.AzureBlob;

/// <summary>
/// Configuration options for Azure Blob Storage.
/// </summary>
public class AzureBlobStorageOptions
{
    /// <summary>
    /// The connection string to the Azure Storage Account.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// The name of the target blob container.
    /// </summary>
    public string ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// If true, checks and creates the container if it does not exist during uploads.
    /// </summary>
    public bool CreateContainerIfNotExists { get; set; } = true;
}
