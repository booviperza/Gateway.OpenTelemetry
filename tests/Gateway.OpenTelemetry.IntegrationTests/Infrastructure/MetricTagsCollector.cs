namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Collects HTTP metric tags captured during integration tests.
/// </summary>
internal sealed class MetricTagsCollector
{
    private readonly List<KeyValuePair<string, object?>> _tags = [];

    public IReadOnlyList<KeyValuePair<string, object?>> Tags
    {
        get
        {
            lock (_tags)
            {
                return _tags.ToList();
            }
        }
    }

    public void Set(
        IEnumerable<KeyValuePair<string, object?>> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        lock (_tags)
        {
            _tags.Clear();
            _tags.AddRange(tags);
        }
    }

    public void Clear()
    {
        lock (_tags)
        {
            _tags.Clear();
        }
    }

    public object? GetValue(
        string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        lock (_tags)
        {
            foreach (KeyValuePair<string, object?> tag in _tags)
            {
                if (string.Equals(
                        tag.Key,
                        name,
                        StringComparison.Ordinal))
                {
                    return tag.Value;
                }
            }

            return null;
        }
    }
}
