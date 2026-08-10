using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Proxy.Features;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.Proxy.Tracing;

/// <summary>
/// Enriches tracing activities with proxy telemetry.
/// </summary>
internal sealed class ProxyTraceEnricher
    : ITraceEnricher
{
    public void Enrich(
        HttpContext httpContext,
        Activity activity)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(activity);

        IProxyTelemetryFeature? feature =
            httpContext.Features.Get<IProxyTelemetryFeature>();

        if (feature is null)
        {
            return;
        }

        SetTag(activity, ProxyTagNames.RouteId, feature.RouteId);

        SetTag(activity, ProxyTagNames.RoutePattern, feature.RoutePattern);

        SetTag(activity, ProxyTagNames.Upstream, feature.Upstream);

        //SetTag(activity, ProxyTagNames.TargetUrl, feature.TargetUrl);

        SetTag(activity, ProxyTagNames.ClientId, feature.ClientId);

        SetTag(activity, ProxyTagNames.CorrelationId, feature.CorrelationId);

        SetTag(activity, ProxyTagNames.CertificateId, feature.CertificateId);

        if (feature.Timeout.HasValue)
        {
            activity.SetTag(
                ProxyTagNames.Timeout,
                feature.Timeout.Value.TotalMilliseconds);
        }

        SetTag(activity, ProxyTagNames.ProxyName, feature.ProxyName);
    }

    private static void SetTag(
        Activity activity,
        string tagName,
        string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            activity.SetTag(tagName, value);
        }
    }
}
