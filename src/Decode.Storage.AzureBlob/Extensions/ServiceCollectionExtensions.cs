using Decode.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Decode.Storage.AzureBlob.Extensions;

/// <summary>
/// Service collection extensions for Azure Blob Storage registration.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure Blob Storage service implementation to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Delegate to configure <see cref="AzureBlobStorageOptions"/>.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddAzureBlobStorage(
        this IServiceCollection services,
        Action<AzureBlobStorageOptions> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.Configure(configureOptions);
        services.TryAddTransient<IStorageService, AzureBlobStorageService>();
        services.TryAddSingleton<IFileValidator, FileSignatureValidator>();

        return services;
    }
}
