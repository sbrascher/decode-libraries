using Microsoft.Extensions.DependencyInjection;

namespace Decode.Notifications.Extensions;

/// <summary>
/// Extension methods for setting up domain notifications in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds domain notification services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDomainNotifications(this IServiceCollection services)
    {
        services.AddScoped<IDomainNotificationContext, DomainNotificationContext>();

        return services;
    }

    /// <summary>
    /// Adds domain notification services to the specified <see cref="IServiceCollection" />.
    /// This is an alias for <see cref="AddDomainNotifications(IServiceCollection)" /> to ensure backward and forward compatibility.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDecodeNotifications(this IServiceCollection services)
    {
        return services.AddDomainNotifications();
    }
}
