using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.AspNetCore.Metrics;

/// <summary>
/// Enriches ASP.NET Core HTTP metric tags.
/// </summary>
public interface IMetricEnricher
{
    /// <summary>
    /// Enriches metric tags for the current HTTP request.
    /// </summary>
    /// <param name="httpContext">
    /// Current HTTP context.
    /// </param>
    /// <param name="tags">
    /// HTTP metric tags.
    /// </param>
    void Enrich(
        HttpContext httpContext,
        ICollection<KeyValuePair<string, object?>> tags);
}
