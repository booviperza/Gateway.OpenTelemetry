using OpenIddict.Server;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Starts timing OpenIddict server requests.
/// </summary>
internal sealed class OpenIddictRequestDurationTelemetryHandler
    : IOpenIddictServerHandler<
        OpenIddictServerEvents.ProcessRequestContext>
{
    internal const string StartTimestampProperty =
        "Gateway.OpenTelemetry.OpenIddict.RequestStartTimestamp";

    internal sealed record RequestStartTimestamp(
        long Value);

    public ValueTask HandleAsync(
        OpenIddictServerEvents.ProcessRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Transaction.Request is null)
        {
            return ValueTask.CompletedTask;
        }

        OpenIddictServerHelpers.SetProperty(
            context.Transaction,
            StartTimestampProperty,
            new RequestStartTimestamp(
                Stopwatch.GetTimestamp()));

        return ValueTask.CompletedTask;
    }
}
