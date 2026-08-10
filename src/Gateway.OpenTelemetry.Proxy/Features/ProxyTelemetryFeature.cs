namespace Gateway.OpenTelemetry.Proxy.Features;

/// <summary>
/// Default implementation of <see cref="IProxyTelemetryFeature"/>.
/// </summary>
public sealed class ProxyTelemetryFeature
    : IProxyTelemetryFeature
{
    public string? RouteId { get; init; }

    public string? RoutePattern { get; init; }

    public string? Upstream { get; init; }

    //public string? TargetUrl { get; init; }

    public string? ClientId { get; init; }

    public string? CorrelationId { get; init; }

    public string? CertificateId { get; init; }

    public TimeSpan? Timeout { get; init; }

    public string? ProxyName { get; init; }
}
