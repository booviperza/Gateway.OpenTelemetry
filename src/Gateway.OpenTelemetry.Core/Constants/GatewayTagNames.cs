namespace Gateway.OpenTelemetry.Core.Constants;

/// <summary>
/// Defines well-known OpenTelemetry tag names used by Gateway.OpenTelemetry.
/// </summary>
public static class GatewayTagNames
{
    /// <summary>
    /// Gateway host.
    /// </summary>
    public const string Host = "gateway.host";

    /// <summary>
    /// Exception type.
    /// </summary>
    public const string ExceptionType = "gateway.exception.type";

    /// <summary>
    /// YARP route identifier.
    /// </summary>
    public const string YarpRouteId = "gateway.yarp.route_id";

    /// <summary>
    /// YARP cluster identifier.
    /// </summary>
    public const string YarpClusterId = "gateway.yarp.cluster_id";

    /// <summary>
    /// YARP destination identifier.
    /// </summary>
    public const string YarpDestinationId = "gateway.yarp.destination_id";
}
