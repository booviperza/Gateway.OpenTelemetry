using OpenTelemetry;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Exports Activities into an <see cref="ActivityCollector"/> for assertions.
/// </summary>
internal sealed class TestActivityExporter : BaseExporter<Activity>
{
    private readonly ActivityCollector _collector;

    public TestActivityExporter(
        ActivityCollector collector)
    {
        ArgumentNullException.ThrowIfNull(collector);

        _collector = collector;
    }

    public override ExportResult Export(
        in Batch<Activity> batch)
    {
        foreach (Activity activity in batch)
        {
            _collector.Add(activity);
        }

        return ExportResult.Success;
    }
}
