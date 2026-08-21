using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration.Authorization;

/// <summary>
/// Records telemetry when an OpenIddict authorization request
/// is rejected.
/// </summary>
internal sealed class AuthorizationDeniedTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessErrorContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public AuthorizationDeniedTelemetryHandler(
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

        if (transaction.EndpointType !=
            OpenIddictServerEndpointType.Authorization)
        {
            return ValueTask.CompletedTask;
        }

        var request = transaction.Request;

        if (request is null)
        {
            return ValueTask.CompletedTask;
        }

        if (string.IsNullOrWhiteSpace(context.Error))
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordAuthorizationDenied(
            context.Error);

        return ValueTask.CompletedTask;
    }
}
