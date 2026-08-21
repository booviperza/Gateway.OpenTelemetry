using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Authorization;

/// <summary>
/// Records duration telemetry when an authorization response
/// is successfully processed.
/// </summary>
internal sealed class AuthorizationResponseDurationTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ApplyAuthorizationResponseContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public AuthorizationResponseDurationTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ApplyAuthorizationResponseContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        OpenIddictRequestDurationRecorder.Record(
            context.Transaction,
            _recorder);

        return ValueTask.CompletedTask;
    }
}
