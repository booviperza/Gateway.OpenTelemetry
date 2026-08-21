using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Token;

/// <summary>
/// Records request duration when a token response is successfully applied.
/// </summary>
internal sealed class TokenResponseDurationTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ApplyTokenResponseContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public TokenResponseDurationTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ApplyTokenResponseContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        OpenIddictRequestDurationRecorder.Record(
            context.Transaction,
            _recorder);

        return ValueTask.CompletedTask;
    }
}
