using Decode.Notifications.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Decode AspNetCore services.
/// </summary>
public static class DecodeServiceCollectionExtensions
{
    /// <summary>
    /// Adds all Decode ASP.NET Core infrastructure services, including Domain Notifications.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDecodeAspNetCore(this IServiceCollection services)
    {
        services.AddDomainNotifications();

        return services;
    }
}