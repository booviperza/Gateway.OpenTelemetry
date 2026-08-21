using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Token;

/// <summary>
/// Records telemetry when OpenIddict processes a successful sign-in
/// that is configured to generate an issued token.
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

        if (!context.GenerateIssuedToken)
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
