namespace Gateway.OpenTelemetry.Core.Constants;

/// <summary>
/// Gateway-specific OpenTelemetry tag names.
/// </summary>
public static class GatewayTagNames
{
    public const string Route =
        "gateway.route";

    public const string Cluster =
        "gateway.cluster";

    public const string Destination =
        "gateway.destination";

    public const string Endpoint =
        "gateway.endpoint";

    public const string Proxy =
        "gateway.proxy";

    public const string Tenant =
        "gateway.tenant";
}
