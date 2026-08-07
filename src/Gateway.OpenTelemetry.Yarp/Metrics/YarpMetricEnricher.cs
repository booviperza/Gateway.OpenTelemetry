using Gateway.OpenTelemetry.AspNetCore.Metrics;

namespace Gateway.OpenTelemetry.Yarp.Metrics;

/// <summary>
/// Adds YARP-specific metric tags.
/// </summary>
internal sealed class YarpMetricEnricher : IMetricEnricher
{
    public void Enrich(
        HttpContext httpContext,
        TagList tags)
    {
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
            tags.Add(
                GatewayTagNames.YarpRouteId,
                routeId);
        }

        string? clusterId =
            feature.Cluster.Config.ClusterId;

        if (!string.IsNullOrWhiteSpace(clusterId))
        {
            tags.Add(
                GatewayTagNames.YarpClusterId,
                clusterId);
        }

        string? destinationId =
            feature.ProxiedDestination?.DestinationId;

        if (!string.IsNullOrWhiteSpace(destinationId))
        {
            tags.Add(
                GatewayTagNames.YarpDestinationId,
                destinationId);
        }
    }
}
