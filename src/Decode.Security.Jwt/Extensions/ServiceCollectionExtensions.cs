using Microsoft.Extensions.DependencyInjection;

namespace Decode.Security.Jwt.Extensions;

/// <summary>
/// Extension methods for registering JWT services in the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds JWT services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">An action to configure the <see cref="JwtOptions"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddJwt(this IServiceCollection services, Action<JwtOptions> configure)
    {
        services.Configure(configure);
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}
