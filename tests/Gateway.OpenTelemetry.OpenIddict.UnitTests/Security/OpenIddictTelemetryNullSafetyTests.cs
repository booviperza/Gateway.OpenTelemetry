using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Token;

/// <summary>
/// Records telemetry when an OpenIddict token request
/// enters the token request validation pipeline.
/// </summary>
internal sealed class TokenRequestTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ValidateTokenRequestContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public TokenRequestTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ValidateTokenRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var request = context.Transaction.Request;

        if (request is null)
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordTokenRequest(
            request.GrantType);

        return ValueTask.CompletedTask;
    }
}
