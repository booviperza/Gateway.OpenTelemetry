using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Extension methods for configuring test exporters.
/// </summary>
internal static class OpenTelemetryBuilderExtensions
{
    public static TracerProviderBuilder AddTestExporter(
        this TracerProviderBuilder builder,
        ActivityCollector collector)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(collector);

        return builder.AddProcessor(
            new SimpleActivityExportProcessor(
                new TestActivityExporter(collector)));
    }
}
