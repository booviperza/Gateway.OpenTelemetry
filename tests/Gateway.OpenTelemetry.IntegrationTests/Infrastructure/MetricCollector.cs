using OpenTelemetry.Metrics;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Collects exported metrics during integration tests.
/// </summary>
internal sealed class MetricCollector
{
    private readonly List<Metric> _metrics = [];

    public IReadOnlyList<Metric> Metrics
        => _metrics;

    public void Add(
        Metric metric)
    {
        ArgumentNullException.ThrowIfNull(metric);

        lock (_metrics)
        {
            _metrics.Add(metric);
        }
    }

    public void Clear()
    {
        lock (_metrics)
        {
            _metrics.Clear();
        }
    }
}
