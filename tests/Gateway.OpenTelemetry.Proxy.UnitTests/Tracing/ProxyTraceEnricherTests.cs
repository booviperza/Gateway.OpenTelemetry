using Gateway.OpenTelemetry.Proxy.Features;
using Gateway.OpenTelemetry.Proxy.Tracing;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.Proxy.UnitTests.Tracing;

public sealed class ProxyTraceEnricherTests
{
    [Fact]
    public void Enrich_Should_AddProxyTagsToActivity()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        httpContext.Features.Set<IProxyTelemetryFeature>(
            new ProxyTelemetryFeature
            {
                RouteId = "route-01",
                RoutePattern = "/api/{**path}",
                Upstream = "https://backend.internal",
                ClientId = "client-01",
                CorrelationId = "correlation-01",
                CertificateId = "certificate-01",
                Timeout = TimeSpan.FromSeconds(30),
                ProxyName = "test-gateway"
            });

        ProxyTraceEnricher enricher = new();

        using Activity activity = new("test");

        activity.Start();

        // Act
        enricher.Enrich(
            httpContext,
            activity);

        // Assert
        Assert.Equal(
            "route-01",
            activity.GetTagItem("gateway.proxy.route_id"));

        Assert.Equal(
            "/api/{**path}",
            activity.GetTagItem("gateway.proxy.route"));

        Assert.Equal(
            "https://backend.internal",
            activity.GetTagItem("gateway.proxy.upstream"));

        Assert.Equal(
            "client-01",
            activity.GetTagItem("gateway.proxy.client_id"));

        Assert.Equal(
            "correlation-01",
            activity.GetTagItem("gateway.proxy.correlation_id"));

        Assert.Equal(
            "certificate-01",
            activity.GetTagItem("gateway.proxy.certificate"));

        Assert.Equal(
            30_000d,
            activity.GetTagItem("gateway.proxy.timeout"));

        Assert.Equal(
            "test-gateway",
            activity.GetTagItem("gateway.proxy.name"));
    }

    [Fact]
    public void Enrich_Should_DoNothing_WhenFeatureIsMissing()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        ProxyTraceEnricher enricher = new();

        using Activity activity = new("test");

        activity.Start();

        // Act
        enricher.Enrich(
            httpContext,
            activity);

        // Assert
        Assert.Null(
            activity.GetTagItem("gateway.proxy.route_id"));

        Assert.Null(
            activity.GetTagItem("gateway.proxy.upstream"));
    }
}
