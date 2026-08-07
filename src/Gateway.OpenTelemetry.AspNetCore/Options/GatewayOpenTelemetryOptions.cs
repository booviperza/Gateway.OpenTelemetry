using OpenTelemetry.Trace;

namespace Gateway.OpenTelemetry.AspNetCore.Options;

/// <summary>
/// Options for configuring Gateway OpenTelemetry.
/// </summary>
public sealed class GatewayOpenTelemetryOptions
{
    /// <summary>
    /// Gets or sets an optional callback that can further configure the
    /// <see cref="TracerProviderBuilder"/>.
    /// </summary>
    public Action<TracerProviderBuilder>? ConfigureTracing { get; set; }
}
