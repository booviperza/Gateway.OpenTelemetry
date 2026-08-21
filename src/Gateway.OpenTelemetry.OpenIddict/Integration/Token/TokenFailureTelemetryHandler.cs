using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Token;

/// <summary>
/// Records telemetry when an OpenIddict token request fails.
/// </summary>
internal sealed class TokenFailureTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessErrorContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public TokenFailureTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var transaction = context.Transaction;

        var request = transaction.Request;

        if (request is null)
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordTokenFailure(
            request.GrantType,
            context.Error);

        return ValueTask.CompletedTask;
    }
}
