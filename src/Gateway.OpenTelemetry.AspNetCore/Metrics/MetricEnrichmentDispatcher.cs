using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Gateway.OpenTelemetry.AspNetCore.Metrics;

/// <summary>
/// Dispatches metric enrichment to the ASP.NET Core HTTP metrics feature.
/// </summary>
internal sealed class MetricEnrichmentDispatcher
{
    private readonly CompositeMetricEnricher _compositeMetricEnricher;

    public MetricEnrichmentDispatcher(
        CompositeMetricEnricher compositeMetricEnricher)
    {
        ArgumentNullException.ThrowIfNull(compositeMetricEnricher);

        _compositeMetricEnricher =
            compositeMetricEnricher;
    }

    public void Enrich(
        HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        IHttpMetricsTagsFeature? metricsFeature =
            httpContext.Features
                .Get<IHttpMetricsTagsFeature>();

        if (metricsFeature is null)
        {
            return;
        }

        if (metricsFeature.MetricsDisabled)
        {
            return;
        }

        _compositeMetricEnricher.Enrich(
            httpContext,
            metricsFeature.Tags);
    }
}
