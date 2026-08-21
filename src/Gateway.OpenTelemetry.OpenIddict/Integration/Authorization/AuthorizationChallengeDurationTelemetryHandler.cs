using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Authorization;

internal sealed class AuthorizationChallengeDurationTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessChallengeContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public AuthorizationChallengeDurationTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessChallengeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        OpenIddictRequestDurationRecorder.Record(
            context.Transaction,
            _recorder);

        return ValueTask.CompletedTask;
    }
}
