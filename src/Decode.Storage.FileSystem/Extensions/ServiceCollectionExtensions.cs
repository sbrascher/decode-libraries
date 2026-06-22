using Decode.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Decode.Storage.FileSystem.Extensions;

/// <summary>
/// Service collection extensions for local file system storage registration.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds local file system storage service implementation to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Delegate to configure <see cref="FileSystemStorageOptions"/>.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddFileSystemStorage(
        this IServiceCollection services,
        Action<FileSystemStorageOptions> configureOptions)
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
        services.TryAddTransient<IStorageService, FileSystemStorageService>();

        return services;
    }
}
