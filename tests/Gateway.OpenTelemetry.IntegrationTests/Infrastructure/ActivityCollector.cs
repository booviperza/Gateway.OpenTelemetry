using System.Diagnostics;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Collects exported activities during integration tests.
/// </summary>
internal sealed class ActivityCollector
{
    private readonly List<Activity> _activities = [];

    public IReadOnlyList<Activity> Activities
        => _activities;

    public void Add(
        Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        lock (_activities)
        {
            _activities.Add(activity);
        }
    }

    public void Clear()
    {
        lock (_activities)
        {
            _activities.Clear();
        }
    }

    public Activity? LastActivity
    {
        get
        {
            lock (_activities)
            {
                return _activities.LastOrDefault();
            }
        }
    }
}
