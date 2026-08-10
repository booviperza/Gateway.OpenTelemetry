namespace Gateway.OpenTelemetry.Proxy.Tracing;

/// <summary>
/// Proxy telemetry tag names.
/// </summary>
internal static class ProxyTagNames
{
    public const string RouteId =
        "gateway.proxy.route_id";

    public const string RoutePattern =
        "gateway.proxy.route";

    public const string Upstream =
        "gateway.proxy.upstream";

    public const string TargetUrl =
        "gateway.proxy.target";

    public const string ClientId =
        "gateway.proxy.client_id";

    public const string CorrelationId =
        "gateway.proxy.correlation_id";

    public const string CertificateId =
        "gateway.proxy.certificate";

    public const string Timeout =
        "gateway.proxy.timeout";

    public const string ProxyName =
        "gateway.proxy.name";
}
