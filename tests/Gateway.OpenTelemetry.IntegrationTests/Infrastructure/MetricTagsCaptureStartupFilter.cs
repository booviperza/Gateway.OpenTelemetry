using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Captures ASP.NET Core HTTP metric tags for integration tests.
/// </summary>
internal sealed class MetricTagsCaptureStartupFilter
    : IStartupFilter
{
    private readonly MetricTagsCollector _collector;

    public MetricTagsCaptureStartupFilter(
        MetricTagsCollector collector)
    {
        ArgumentNullException.ThrowIfNull(collector);

        _collector = collector;
    }

    public Action<IApplicationBuilder> Configure(
        Action<IApplicationBuilder> next)
    {
        ArgumentNullException.ThrowIfNull(next);

        return app =>
        {
            app.Use(
                async (
                    context,
                    nextMiddleware) =>
                {
                    await nextMiddleware(context);

                    IHttpMetricsTagsFeature? feature =
                        context.Features.Get<IHttpMetricsTagsFeature>();

                    if (feature is not null)
                    {
                        _collector.Set(feature.Tags);
                    }
                });

            next(app);
        };
    }
}
