namespace Gateway.OpenTelemetry.OpenIddict.Options;

/// <summary>
/// Configures OpenIddict telemetry behavior.
/// </summary>
public sealed class OpenIddictTelemetryOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether OpenIddict-specific
    /// metrics are enabled.
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether OpenIddict-specific
    /// tracing is enabled.
    /// </summary>
    public bool EnableTracing { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether client identifiers
    /// may be included as telemetry dimensions.
    /// </summary>
    /// <remarks>
    /// Disabled by default because client identifiers may introduce
    /// unnecessary cardinality or disclose internal information.
    /// </remarks>
    public bool EnableClientIdDimension { get; set; } = false;
}
