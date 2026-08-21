using Gateway.OpenTelemetry.OpenIddict.Security;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.OpenIddict.Metrics;

/// <summary>
/// Records security-safe OpenIddict telemetry.
/// </summary>
internal sealed class OpenIddictTelemetryRecorder
{
    private readonly OpenIddictMetrics _metrics;

    public OpenIddictTelemetryRecorder(
        OpenIddictMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);

        _metrics = metrics;
    }

    public void RecordServerRequest(
        string endpoint)
    {
        var tags = CreateTags(
            endpoint: endpoint);

        _metrics.ServerRequests.Add(
            1,
            tags);
    }

    public void RecordServerRequestDuration(
        string endpoint,
        double durationSeconds)
    {
        if (durationSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(durationSeconds));
        }

        var tags = CreateTags(
            endpoint: endpoint);

        _metrics.ServerRequestDuration.Record(
            durationSeconds,
            tags);
    }

    public void RecordTokenRequest(
        string? grantType)
    {
        var normalizedGrantType =
            OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(grantType);

        var tags = CreateTags(
            grantType: normalizedGrantType);

        _metrics.TokenRequests.Add(
            1,
            tags);
    }

    public void RecordTokenIssued(
        string? grantType)
    {
        var normalizedGrantType =
            OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(grantType);

        var tags = CreateTags(
            grantType: normalizedGrantType,
            result: OpenIddictTelemetryResults.Success);

        _metrics.TokenIssued.Add(
            1,
            tags);
    }

    public void RecordTokenFailure(
        string? grantType,
        string? error)
    {
        var normalizedGrantType =
            OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(grantType);

        var safeError =
            OpenIddictTelemetryErrorSanitizer
                .Sanitize(error);

        var tags = CreateTags(
            grantType: normalizedGrantType,
            result: OpenIddictTelemetryResults.Failure,
            error: safeError);

        _metrics.TokenFailures.Add(
            1,
            tags);
    }

    public void RecordAuthorizationRequest(
        string result)
    {
        var tags = CreateTags(
            endpoint: "authorize",
            result: result);

        _metrics.AuthorizationRequests.Add(
            1,
            tags);
    }

    public void RecordAuthorizationDenied(
        string error)
    {
        var safeError =
            OpenIddictTelemetryErrorSanitizer
                .Sanitize(error);

        var tags = CreateTags(
            endpoint: "authorize",
            result: OpenIddictTelemetryResults.Denied,
            error: safeError);

        _metrics.AuthorizationDenied.Add(
            1,
            tags);
    }

    private static TagList CreateTags(
        string? endpoint = null,
        string? grantType = null,
        string? result = null,
        string? error = null)
    {
        var tags = new TagList();

        if (!string.IsNullOrWhiteSpace(endpoint))
        {
            tags.Add(
                OpenIddictTagNames.Endpoint,
                endpoint);
        }

        if (!string.IsNullOrWhiteSpace(grantType))
        {
            tags.Add(
                OpenIddictTagNames.GrantType,
                grantType);
        }

        if (!string.IsNullOrWhiteSpace(result))
        {
            tags.Add(
                OpenIddictTagNames.Result,
                result);
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            tags.Add(
                OpenIddictTagNames.Error,
                error);
        }

        return tags;
    }
}
