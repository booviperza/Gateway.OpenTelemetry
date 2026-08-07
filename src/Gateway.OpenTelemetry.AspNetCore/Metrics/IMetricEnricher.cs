using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.Metrics;

/// <summary>
/// Enriches ASP.NET Core metric tags.
/// </summary>
public interface IMetricEnricher
{
    /// <summary>
    /// Enriches metric tags.
    /// </summary>
    /// <param name="httpContext">
    /// Current HTTP request context.
    /// </param>
    /// <param name="tags">
    /// Metric tags.
    /// </param>
    void Enrich(
        HttpContext httpContext,
        TagList tags);
}
