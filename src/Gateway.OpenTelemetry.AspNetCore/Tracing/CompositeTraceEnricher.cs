using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.Tracing;

/// <summary>
/// Executes all registered trace enrichers.
/// </summary>
internal sealed class CompositeTraceEnricher
{
    private readonly IReadOnlyList<ITraceEnricher> _enrichers;

    public CompositeTraceEnricher(
        IEnumerable<ITraceEnricher> enrichers)
    {
        ArgumentNullException.ThrowIfNull(enrichers);

        _enrichers = enrichers.ToList();
    }

    public void Enrich(
        HttpContext httpContext,
        Activity activity)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(activity);

        foreach (ITraceEnricher enricher in _enrichers)
        {
            enricher.Enrich(httpContext, activity);
        }
    }
}
