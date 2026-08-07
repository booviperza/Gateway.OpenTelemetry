using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.Tracing;

/// <summary>
/// Enriches the current tracing <see cref="Activity"/>.
/// </summary>
public interface ITraceEnricher
{
    /// <summary>
    /// Enriches the specified activity.
    /// </summary>
    /// <param name="httpContext">
    /// Current HTTP request context.
    /// </param>
    /// <param name="activity">
    /// Activity to enrich.
    /// </param>
    void Enrich(
        HttpContext httpContext,
        Activity activity);
}
