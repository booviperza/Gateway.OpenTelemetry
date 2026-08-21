using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Records telemetry for OpenIddict server requests.
/// </summary>
internal sealed class OpenIddictRequestTelemetryHandler
    : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessRequestContext>
{
    private readonly Metrics.OpenIddictTelemetryRecorder _recorder;

    public OpenIddictRequestTelemetryHandler(
        Metrics.OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Transaction.Request is null)
        {
            return ValueTask.CompletedTask;
        }

        var endpoint = ResolveEndpoint(context);

        if (endpoint is null)
        {
            return ValueTask.CompletedTask;
        }

        _recorder.RecordServerRequest(endpoint);

        return ValueTask.CompletedTask;
    }

    private static string? ResolveEndpoint(
        OpenIddictServerEvents.ProcessRequestContext context)
    {
        return context.Transaction.EndpointType switch
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
