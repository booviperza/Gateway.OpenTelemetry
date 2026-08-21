using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Completes duration telemetry for an OpenIddict server request.
/// </summary>
internal static class OpenIddictRequestDurationRecorder
{
    public static void Record(
    OpenIddictServerTransaction transaction,
    OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(recorder);

        var start =
            OpenIddictServerHelpers.GetProperty<
                OpenIddictRequestDurationTelemetryHandler
                    .RequestStartTimestamp>(
                transaction,
                OpenIddictRequestDurationTelemetryHandler
                    .StartTimestampProperty);

        if (start is not
            OpenIddictRequestDurationTelemetryHandler
                .RequestStartTimestamp timestamp)
        {
            return;
        }

        var endpoint = ResolveEndpoint(transaction);

        if (endpoint is null)
        {
            return;
        }

        var elapsed =
            Stopwatch.GetElapsedTime(timestamp.Value);

        recorder.RecordServerRequestDuration(
            endpoint,
            elapsed.TotalSeconds);
    }

    private static string? ResolveEndpoint(
        OpenIddictServerTransaction transaction)
    {
        return transaction.EndpointType switch
        {
            OpenIddictServerEndpointType.Authorization =>
                "authorize",

            OpenIddictServerEndpointType.Token =>
                "token",

            OpenIddictServerEndpointType.Introspection =>
                "introspection",

            OpenIddictServerEndpointType.Revocation =>
                "revocation",

            OpenIddictServerEndpointType.EndSession =>
                "end_session",

            _ => "other"
        };
    }
}
