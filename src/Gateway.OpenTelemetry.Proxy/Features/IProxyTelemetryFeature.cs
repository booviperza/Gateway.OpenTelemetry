namespace Gateway.OpenTelemetry.Proxy.Features;

/// <summary>
/// Provides proxy telemetry information for the current request.
/// </summary>
public interface IProxyTelemetryFeature
{
    /// <summary>
    /// Gets the matched route identifier.
    /// </summary>
    string? RouteId { get; }

    /// <summary>
    /// Gets the matched route pattern.
    /// </summary>
    string? RoutePattern { get; }

    /// <summary>
    /// Gets the selected upstream endpoint.
    /// </summary>
    string? Upstream { get; }

    /// <summary>
    /// Gets the client identifier.
    /// </summary>
    string? ClientId { get; }

    /// <summary>
    /// Gets the correlation identifier.
    /// </summary>
    string? CorrelationId { get; }

    /// <summary>
    /// Gets the client certificate identifier.
    /// </summary>
    string? CertificateId { get; }

    /// <summary>
    /// Gets the configured request timeout.
    /// </summary>
    TimeSpan? Timeout { get; }

    /// <summary>
    /// Gets the proxy implementation name.
    /// </summary>
    string? ProxyName { get; }
}
