namespace Gateway.OpenTelemetry.OpenIddict.Security;

/// <summary>
/// Defines the security boundary for OpenIddict telemetry.
/// </summary>
internal static class OpenIddictTelemetrySecurityPolicy
{
    /// <summary>
    /// Determines whether a telemetry field is explicitly allowed.
    /// </summary>
    /// <param name="name">
    /// Telemetry field name.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the field is safe to emit.
    /// </returns>
    public static bool IsAllowed(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return name switch
        {
            "endpoint" => true,
            "grant_type" => true,
            "result" => true,
            "error" => true,
            "http_method" => true,
            "http_status_code" => true,

            _ => false
        };
    }
}
