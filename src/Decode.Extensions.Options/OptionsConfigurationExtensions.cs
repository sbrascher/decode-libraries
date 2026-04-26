using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Decode.Extensions.Options;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> and <see cref="IConfiguration"/> to handle validated Options pattern.
/// </summary>
public static class OptionsConfigurationExtensions
{
    /// <summary>
    /// Registers and validates a configuration using the Options pattern, requiring the section to exist in the configuration provider.
    /// The section name must be identical to the class name <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of options to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The configuration provider.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the required section is missing.</exception>
    public static IServiceCollection AddValidatedOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class
    {
        IConfigurationSection section = GetRequiredSection<T>(configuration);
        services.Configure<T>(section);

        return services;
    }

    /// <summary>
    /// Returns the instantiated value of a configuration section. Throws an exception if it does not exist.
    /// </summary>
    /// <typeparam name="T">The type of options to retrieve.</typeparam>
    /// <param name="configuration">The configuration provider.</param>
    /// <returns>The instantiated configuration object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the required section is missing.</exception>
    public static T GetValidatedSectionValue<T>(this IConfiguration configuration) where T : class
    {
        IConfigurationSection section = GetRequiredSection<T>(configuration);
        return section.Get<T>()!;
    }

    private static IConfigurationSection GetRequiredSection<T>(IConfiguration configuration) where T : class
    {
        string sectionName = typeof(T).Name;
        IConfigurationSection section = configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"[Configuration Failure] The section '{sectionName}' was not found in the configuration provider.");
        }

        return section;
    }
}
