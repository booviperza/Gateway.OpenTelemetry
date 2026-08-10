using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Gateway.OpenTelemetry.AspNetCore.Middleware;

/// <summary>
/// Automatically adds the Gateway metric enrichment middleware
/// to the ASP.NET Core request pipeline.
/// </summary>
internal sealed class MetricEnrichmentStartupFilter
    : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(
        Action<IApplicationBuilder> next)
    {
        ArgumentNullException.ThrowIfNull(next);

        return app =>
        {
            app.UseMiddleware<MetricEnrichmentMiddleware>();

            next(app);
        };
    }
}
