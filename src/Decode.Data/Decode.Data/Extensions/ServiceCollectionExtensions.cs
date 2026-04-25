using Decode.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

namespace Decode.Data.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(
        this IServiceCollection services,
        Func<IServiceProvider, DbConnection> connectionFactory)
    {
        services.AddSingleton<DbConnectionFactory>(sp => () => connectionFactory(sp));

        services.TryAddScoped<IDbSession, DbSession>();
        services.TryAddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}