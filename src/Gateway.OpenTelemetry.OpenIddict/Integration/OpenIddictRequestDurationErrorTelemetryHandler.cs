using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Records duration telemetry when an OpenIddict request
/// completes with an error.
/// </summary>
internal sealed class OpenIddictRequestDurationErrorTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessErrorContext>
{
    private readonly OpenIddictTelemetryRecorder _recorder;

    public OpenIddictRequestDurationErrorTelemetryHandler(
        OpenIddictTelemetryRecorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        _recorder = recorder;
    }

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        OpenIddictRequestDurationRecorder.Record(
            context.Transaction,
            _recorder);

        return ValueTask.CompletedTask;
    }
}
