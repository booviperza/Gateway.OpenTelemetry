namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Provides extensions for integrating Gateway.OpenTelemetry
/// handlers with the OpenIddict server pipeline.
/// </summary>
public static class OpenIddictServerBuilderExtensions
{
    /// <summary>
    /// Registers Gateway.OpenTelemetry OpenIddict telemetry
    /// handlers with the server pipeline.
    /// </summary>
    /// <param name="builder">
    /// OpenIddict server builder.
    /// </param>
    /// <returns>
    /// The same builder instance.
    /// </returns>
    public static OpenIddictServerBuilder
        UseGatewayOpenTelemetry(
            this OpenIddictServerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddEventHandler(
            OpenIddictTelemetryHandlers.Request);

        builder.AddEventHandler(
            OpenIddictTelemetryHandlers.TokenRequest);

        builder.AddEventHandler(
            OpenIddictTelemetryHandlers.TokenIssued);

        builder.AddEventHandler(
            OpenIddictTelemetryHandlers.TokenFailure);

        return builder;
    }
}
