using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Proxy.Tracing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gateway.OpenTelemetry.Proxy.DependencyInjection;

/// <summary>
/// Provides extension methods for registering
/// Gateway.OpenTelemetry proxy services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Gateway.OpenTelemetry proxy integration.
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <returns>
    /// The same service collection instance.
    /// </returns>
    public static IServiceCollection AddGatewayProxyOpenTelemetry(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddGatewayOpenTelemetry();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                ITraceEnricher,
                ProxyTraceEnricher>());

        return services;
    }
}
