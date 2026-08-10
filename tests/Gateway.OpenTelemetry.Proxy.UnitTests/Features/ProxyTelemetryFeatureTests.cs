using Gateway.OpenTelemetry.Proxy.Features;

namespace Gateway.OpenTelemetry.Proxy.UnitTests.Features;

public sealed class ProxyTelemetryFeatureTests
{
    [Fact]
    public void Properties_Should_Return_ConfiguredValues()
    {
        // Arrange
        TimeSpan timeout = TimeSpan.FromSeconds(30);

        ProxyTelemetryFeature feature = new()
        {
            RouteId = "route-01",
            RoutePattern = "/api/{**path}",
            Upstream = "https://backend.internal",
            ClientId = "client-01",
            CorrelationId = "correlation-01",
            CertificateId = "certificate-01",
            Timeout = timeout,
            ProxyName = "test-gateway"
        };

        // Assert
        Assert.Equal("route-01", feature.RouteId);
        Assert.Equal("/api/{**path}", feature.RoutePattern);
        Assert.Equal("https://backend.internal", feature.Upstream);
        Assert.Equal("client-01", feature.ClientId);
        Assert.Equal("correlation-01", feature.CorrelationId);
        Assert.Equal("certificate-01", feature.CertificateId);
        Assert.Equal(timeout, feature.Timeout);
        Assert.Equal("test-gateway", feature.ProxyName);
    }
}
