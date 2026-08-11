using Gateway.OpenTelemetry.AspNetCore.Metrics;
using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Proxy.DependencyInjection;
using Gateway.OpenTelemetry.Proxy.Metrics;
using Gateway.OpenTelemetry.Proxy.Tracing;

using Microsoft.Extensions.DependencyInjection;

namespace Gateway.OpenTelemetry.Proxy.UnitTests.DependencyInjection;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGatewayProxyOpenTelemetry_ShouldRegisterProxyTraceEnricher()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddGatewayProxyOpenTelemetry();

        using ServiceProvider provider =
            services.BuildServiceProvider();

        // Assert
        IEnumerable<ITraceEnricher> enrichers =
            provider.GetServices<ITraceEnricher>();

        Assert.Contains(
            enrichers,
            enricher => enricher is ProxyTraceEnricher);
    }

    [Fact]
    public void AddGatewayProxyOpenTelemetry_ShouldRegisterProxyMetricEnricher()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddGatewayProxyOpenTelemetry();

        using ServiceProvider provider =
            services.BuildServiceProvider();

        // Assert
        IEnumerable<IMetricEnricher> enrichers =
            provider.GetServices<IMetricEnricher>();

        Assert.Contains(
            enrichers,
            enricher => enricher is ProxyMetricEnricher);
    }

    [Fact]
    public void AddGatewayProxyOpenTelemetry_ShouldRegisterCoreAspNetCoreServices()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddGatewayProxyOpenTelemetry();

        // Assert
        using ServiceProvider provider =
            services.BuildServiceProvider();

        Assert.NotNull(
            provider.GetService<ITraceEnricher>());
    }
}
