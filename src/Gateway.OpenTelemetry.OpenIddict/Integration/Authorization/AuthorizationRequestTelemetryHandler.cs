using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Authorization;

/// <summary>
/// Records telemetry when an OpenIddict authorization request
/// enters the authorization request validation pipeline.
/// </summary>
internal sealed class AuthorizationRequestTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ValidateAuthorizationRequestContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public AuthorizationRequestTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ValidateAuthorizationRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var request = context.Transaction.Request;

        if (request is null)
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordAuthorizationRequest(
            "received");

        return ValueTask.CompletedTask;
    }
}
