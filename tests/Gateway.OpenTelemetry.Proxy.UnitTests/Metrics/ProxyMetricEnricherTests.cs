using Gateway.OpenTelemetry.Proxy.Features;
using Gateway.OpenTelemetry.Proxy.Metrics;

using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.Proxy.UnitTests.Metrics;

public sealed class ProxyMetricEnricherTests
{
    [Fact]
    public void Enrich_Should_AddProxyMetricTags()
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

        ProxyMetricEnricher enricher = new();

        List<KeyValuePair<string, object?>> tags = [];

        // Act

        enricher.Enrich(
            httpContext,
            tags);

        // Assert

        Assert.Equal(
            "route-01",
            GetTag(tags, "gateway.proxy.route_id"));

        Assert.Equal(
            "/api/{**path}",
            GetTag(tags, "gateway.proxy.route"));

        Assert.Equal(
            "https://backend.internal",
            GetTag(tags, "gateway.proxy.upstream"));

        Assert.Equal(
            "test-gateway",
            GetTag(tags, "gateway.proxy.name"));
    }

    [Fact]
    public void Enrich_Should_DoNothing_WhenFeatureIsMissing()
    {
        // Arrange

        DefaultHttpContext httpContext = new();

        ProxyMetricEnricher enricher = new();

        List<KeyValuePair<string, object?>> tags = [];

        // Act

        enricher.Enrich(
            httpContext,
            tags);

        // Assert

        Assert.Empty(tags);
    }

    [Fact]
    public void Enrich_Should_NotAddHighCardinalityTags()
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

        ProxyMetricEnricher enricher = new();

        List<KeyValuePair<string, object?>> tags = [];

        // Act

        enricher.Enrich(
            httpContext,
            tags);

        // Assert

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.client_id");

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.correlation_id");

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.certificate");

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.timeout");
    }

    [Fact]
    public void Enrich_Should_IgnoreEmptyValues()
    {
        // Arrange

        DefaultHttpContext httpContext = new();

        httpContext.Features.Set<IProxyTelemetryFeature>(
            new ProxyTelemetryFeature
            {
                RouteId = "route-01",
                RoutePattern = null,
                Upstream = "",
                ClientId = null,
                CorrelationId = null,
                CertificateId = null,
                Timeout = null,
                ProxyName = "test-gateway"
            });

        ProxyMetricEnricher enricher = new();

        List<KeyValuePair<string, object?>> tags = [];

        // Act

        enricher.Enrich(
            httpContext,
            tags);

        // Assert

        Assert.Equal(
            "route-01",
            GetTag(tags, "gateway.proxy.route_id"));

        Assert.Equal(
            "test-gateway",
            GetTag(tags, "gateway.proxy.name"));

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.route");

        Assert.DoesNotContain(
            tags,
            tag => tag.Key == "gateway.proxy.upstream");
    }

    private static object? GetTag(
        IEnumerable<KeyValuePair<string, object?>> tags,
        string name)
    {
        return tags
            .FirstOrDefault(tag => tag.Key == name)
            .Value;
    }
}
