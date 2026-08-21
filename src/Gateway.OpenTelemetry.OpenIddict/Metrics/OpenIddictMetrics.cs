using System.Diagnostics.Metrics;

namespace Gateway.OpenTelemetry.OpenIddict.Metrics;

/// <summary>
/// Defines OpenIddict-specific metric instruments.
/// </summary>
internal sealed class OpenIddictMetrics
{
    public OpenIddictMetrics(Meter meter)
    {
        ArgumentNullException.ThrowIfNull(meter);

        ServerRequests = meter.CreateCounter<long>(
            OpenIddictMetricNames.ServerRequests,
            unit: "{request}",
            description: "Number of OpenIddict server requests.");

        ServerRequestDuration = meter.CreateHistogram<double>(
            OpenIddictMetricNames.ServerRequestDuration,
            unit: "s",
            description: "Duration of OpenIddict server requests.");

        TokenRequests = meter.CreateCounter<long>(
            OpenIddictMetricNames.TokenRequests,
            unit: "{request}",
            description: "Number of OpenIddict token requests.");

        TokenIssued = meter.CreateCounter<long>(
            OpenIddictMetricNames.TokenIssued,
            unit: "{token}",
            description: "Number of tokens issued by OpenIddict.");

        TokenFailures = meter.CreateCounter<long>(
            OpenIddictMetricNames.TokenFailures,
            unit: "{failure}",
            description: "Number of failed OpenIddict token requests.");

        AuthorizationRequests = meter.CreateCounter<long>(
            OpenIddictMetricNames.AuthorizationRequests,
            unit: "{request}",
            description: "Number of OpenIddict authorization requests.");

        AuthorizationDenied = meter.CreateCounter<long>(
            OpenIddictMetricNames.AuthorizationDenied,
            unit: "{denial}",
            description: "Number of denied OpenIddict authorization requests.");
    }

    public Counter<long> ServerRequests { get; }

    public Histogram<double> ServerRequestDuration { get; }

    public Counter<long> TokenRequests { get; }

    public Counter<long> TokenIssued { get; }

    public Counter<long> TokenFailures { get; }

    public Counter<long> AuthorizationRequests { get; }

    public Counter<long> AuthorizationDenied { get; }
}
