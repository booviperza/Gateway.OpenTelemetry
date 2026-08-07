using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.AspNetCore.Metrics;
using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Yarp.Metrics;
using Gateway.OpenTelemetry.Yarp.Tracing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gateway.OpenTelemetry.Yarp.DependencyInjection;

/// <summary>
/// Provides extension methods for registering
/// Gateway.OpenTelemetry YARP services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Gateway.OpenTelemetry YARP integration.
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <returns>
    /// The same service collection instance.
    /// </returns>
    public static IServiceCollection AddGatewayYarpOpenTelemetry(
    this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddGatewayOpenTelemetry();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                ITraceEnricher,
                YarpTraceEnricher>());

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IMetricEnricher,
                YarpMetricEnricher>());

        return services;
    }
}
