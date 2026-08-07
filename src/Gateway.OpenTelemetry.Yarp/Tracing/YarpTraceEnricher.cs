using Gateway.OpenTelemetry.AspNetCore.Tracing;

namespace Gateway.OpenTelemetry.Yarp.Tracing;

/// <summary>
/// Adds YARP-specific tags to the current Activity.
/// </summary>
internal sealed class YarpTraceEnricher : ITraceEnricher
{
    public void Enrich(
        HttpContext httpContext,
        Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);
        ArgumentNullException.ThrowIfNull(httpContext);

        IReverseProxyFeature? feature =
            httpContext.Features.Get<IReverseProxyFeature>();

        if (feature is null)
        {
            return;
        }

        string? routeId =
            feature.Route.Config.RouteId;

        if (!string.IsNullOrWhiteSpace(routeId))
        {
            activity.SetTag(
                GatewayTagNames.YarpRouteId,
                routeId);
        }

        string? clusterId =
            feature.Cluster.Config.ClusterId;

        if (!string.IsNullOrWhiteSpace(clusterId))
        {
            activity.SetTag(
                GatewayTagNames.YarpClusterId,
                clusterId);
        }

        string? destinationId =
            feature.ProxiedDestination?.DestinationId;

        if (!string.IsNullOrWhiteSpace(destinationId))
        {
            activity.SetTag(
                GatewayTagNames.YarpDestinationId,
                destinationId);
        }
    }
}
