using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.OpenIddict.Integration;
using Gateway.OpenTelemetry.OpenIddict.Integration.Token;
using Gateway.OpenTelemetry.OpenIddict.Metrics;
using Gateway.OpenTelemetry.OpenIddict.Options;

namespace Gateway.OpenTelemetry.OpenIddict.DependencyInjection;

/// <summary>
/// Provides extension methods for registering
/// Gateway.OpenTelemetry OpenIddict services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Gateway.OpenTelemetry OpenIddict integration.
    /// </summary>
    /// <param name="services">
    /// Service collection.
    /// </param>
    /// <returns>
    /// The same service collection instance.
    /// </returns>
    public static IServiceCollection AddGatewayOpenIddictOpenTelemetry(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddGatewayOpenTelemetry();

        services.AddOptions<OpenIddictTelemetryOptions>();

        services.AddSingleton(
            static _ => OpenIddictMeter.Create());

        services.AddSingleton<OpenIddictMetrics>();

        services.AddSingleton<OpenIddictTelemetryRecorder>();

        services.AddSingleton<OpenIddictRequestTelemetryHandler>();

        services.AddSingleton<TokenRequestTelemetryHandler>();

        return services;
    }
}
