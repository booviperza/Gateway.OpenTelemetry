using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.Metrics;

/// <summary>
/// Executes all registered metric enrichers.
/// </summary>
internal sealed class CompositeMetricEnricher
{
    private readonly IReadOnlyList<IMetricEnricher> _enrichers;

    public CompositeMetricEnricher(
        IEnumerable<IMetricEnricher> enrichers)
    {
        ArgumentNullException.ThrowIfNull(enrichers);

        _enrichers = enrichers.ToList();
    }

    public void Enrich(
        HttpContext httpContext,
        TagList tags)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        foreach (IMetricEnricher enricher in _enrichers)
        {
            enricher.Enrich(httpContext, tags);
        }
    }
}
