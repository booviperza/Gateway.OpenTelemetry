using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Token;

/// <summary>
/// Records telemetry when OpenIddict successfully processes
/// a sign-in operation that generates an access token.
/// </summary>
internal sealed class TokenIssuedTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessSignInContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public TokenIssuedTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessSignInContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.GenerateAccessToken)
        {
            return ValueTask.CompletedTask;
        }

        var request = context.Request;

        if (request is null)
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordTokenIssued(
            request.GrantType);

        return ValueTask.CompletedTask;
    }
}
