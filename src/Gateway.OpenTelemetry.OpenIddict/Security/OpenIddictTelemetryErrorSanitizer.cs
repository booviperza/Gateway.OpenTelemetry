namespace Gateway.OpenTelemetry.OpenIddict.Security;

/// <summary>
/// Normalizes OpenIddict errors before they are emitted as telemetry.
/// </summary>
internal static class OpenIddictTelemetryErrorSanitizer
{
    private static readonly HashSet<string> AllowedErrors =
        new(StringComparer.Ordinal)
        {
            "invalid_client",
            "invalid_grant",
            "invalid_request",
            "invalid_scope",
            "unauthorized_client",
            "unsupported_grant_type",
            "unsupported_response_type",
            "access_denied",
            "server_error",
            "temporarily_unavailable"
        };

    public static string? Sanitize(string? error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return null;
        }

        return AllowedErrors.Contains(error)
            ? error
            : "unknown";
    }
}
