using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Decode.Security.ApiKey.Extensions;

/// <summary>
/// Extension methods for registering API Key authentication services in the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds API Key authentication services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TValidator">The type of the validator that implements <see cref="IApiKeyValidator"/>.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">An action to configure the <see cref="ApiKeyOptions"/>.</param>
    /// <returns>The updated <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddApiKey<TValidator>(
        this IServiceCollection services,
        Action<ApiKeyOptions>? configureOptions = null)
        where TValidator : class, IApiKeyValidator
    {
        services.AddScoped<IApiKeyValidator, TValidator>();

        var options = new ApiKeyOptions();
        configureOptions?.Invoke(options);

        return services.AddAuthentication(options.Scheme)
            .AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>(
                options.Scheme,
                configureOptions);
    }
}
