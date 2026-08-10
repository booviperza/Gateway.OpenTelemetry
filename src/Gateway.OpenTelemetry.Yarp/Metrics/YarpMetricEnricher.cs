using Gateway.OpenTelemetry.AspNetCore.Metrics;

namespace Gateway.OpenTelemetry.Yarp.Metrics;

/// <summary>
/// Adds YARP-specific metric tags.
/// </summary>
internal sealed class YarpMetricEnricher
    : IMetricEnricher
{
    public void Enrich(
        HttpContext httpContext,
        ICollection<KeyValuePair<string, object?>> tags)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(tags);

        IReverseProxyFeature? proxyFeature =
            httpContext.Features
                .Get<IReverseProxyFeature>();

        if (proxyFeature is null)
        {
            return;
        }

        string? routeId =
            proxyFeature.Route?.Config?.RouteId;

        if (!string.IsNullOrWhiteSpace(routeId))
        {
            tags.Add(
                new KeyValuePair<string, object?>(
                    "gateway.yarp.route_id",
                    routeId));
        }

        string? clusterId =
            proxyFeature.Cluster?.Config?.ClusterId;

        if (!string.IsNullOrWhiteSpace(clusterId))
        {
            tags.Add(
                new KeyValuePair<string, object?>(
                    "gateway.yarp.cluster_id",
                    clusterId));
        }

        string? destinationId =
            proxyFeature.ProxiedDestination?.DestinationId;

        if (!string.IsNullOrWhiteSpace(destinationId))
        {
            tags.Add(
                new KeyValuePair<string, object?>(
                    "gateway.yarp.destination_id",
                    destinationId));
        }
    }
}
