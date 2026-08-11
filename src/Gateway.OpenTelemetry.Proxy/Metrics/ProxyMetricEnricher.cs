using Gateway.OpenTelemetry.AspNetCore.Metrics;
using Gateway.OpenTelemetry.Proxy.Features;
using Gateway.OpenTelemetry.Proxy.Tracing;
using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.Proxy.Metrics;

/// <summary>
/// Adds custom proxy-specific metric tags.
/// </summary>
internal sealed class ProxyMetricEnricher : IMetricEnricher
{
    public void Enrich(
            HttpContext httpContext,
            ICollection<KeyValuePair<string, object?>> tags)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(tags);

        IProxyTelemetryFeature? feature =
            httpContext.Features.Get<IProxyTelemetryFeature>();

        if (feature is null)
        {
            return;
        }

        AddTag(
            tags,
            ProxyTagNames.RouteId,
            feature.RouteId);

        AddTag(
            tags,
            ProxyTagNames.RoutePattern,
            feature.RoutePattern);

        AddTag(
            tags,
            ProxyTagNames.Upstream,
            feature.Upstream);

        AddTag(
            tags,
            ProxyTagNames.ProxyName,
            feature.ProxyName);
    }

    private static void AddTag(
        ICollection<KeyValuePair<string, object?>> tags,
        string tagName,
        string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            tags.Add(
                new KeyValuePair<string, object?>(
                    tagName,
                    value));
        }
    }
}
