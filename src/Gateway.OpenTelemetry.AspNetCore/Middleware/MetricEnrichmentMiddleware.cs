using Gateway.OpenTelemetry.AspNetCore.Metrics;
using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.AspNetCore.Middleware;

/// <summary>
/// Enriches ASP.NET Core HTTP metrics.
/// </summary>
internal sealed class MetricEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public MetricEnrichmentMiddleware(
        RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        MetricEnrichmentDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(dispatcher);

        await _next(context);

        dispatcher.Enrich(context);
    }
}
