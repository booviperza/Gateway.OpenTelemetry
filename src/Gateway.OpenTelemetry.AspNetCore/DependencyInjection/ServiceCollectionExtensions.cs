using Gateway.OpenTelemetry.AspNetCore.Metrics;
using Gateway.OpenTelemetry.AspNetCore.Middleware;
using Gateway.OpenTelemetry.AspNetCore.Options;
using Gateway.OpenTelemetry.AspNetCore.Tracing;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gateway.OpenTelemetry.AspNetCore.DependencyInjection;

/// <summary>
/// Provides extension methods for registering
/// Gateway.OpenTelemetry ASP.NET Core services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Gateway.OpenTelemetry ASP.NET Core integration.
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <returns>
    /// The same service collection instance.
    /// </returns>
    public static IServiceCollection AddGatewayOpenTelemetry(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Tracing

        services.TryAddSingleton<
            CompositeTraceEnricher>();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IConfigureOptions<AspNetCoreTraceInstrumentationOptions>,
                ConfigureAspNetCoreTraceInstrumentationOptions>());

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                ITraceEnricher,
                ExceptionTraceEnricher>());

        // Metrics

        services.TryAddSingleton<
            CompositeMetricEnricher>();

        services.TryAddSingleton<
            MetricEnrichmentDispatcher>();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IStartupFilter,
                MetricEnrichmentStartupFilter>());

        return services;
    }
}
