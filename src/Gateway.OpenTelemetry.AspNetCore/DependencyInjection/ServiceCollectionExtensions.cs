using Gateway.OpenTelemetry.AspNetCore.Options;
using Gateway.OpenTelemetry.AspNetCore.Tracing;

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

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IConfigureOptions<AspNetCoreTraceInstrumentationOptions>,
                ConfigureAspNetCoreTraceInstrumentationOptions>());

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                ITraceEnricher,
                ExceptionTraceEnricher>());

        // Metrics registration will be added after the
        // OpenTelemetry Metrics spike is completed.

        return services;
    }
}
