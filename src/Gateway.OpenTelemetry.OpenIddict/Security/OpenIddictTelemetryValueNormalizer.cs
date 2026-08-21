namespace Gateway.OpenTelemetry.OpenIddict.Security;

/// <summary>
/// Normalizes OpenIddict values before they are emitted as telemetry.
/// </summary>
internal static class OpenIddictTelemetryValueNormalizer
{
    public static string NormalizeGrantType(
        string? grantType)
    {
        if (string.IsNullOrWhiteSpace(grantType))
        {
            return "unknown";
        }

        return grantType switch
        {
            "authorization_code" =>
                "authorization_code",

            "refresh_token" =>
                "refresh_token",

            "client_credentials" =>
                "client_credentials",

            "password" =>
                "password",

            "urn:ietf:params:oauth:grant-type:device_code" =>
                "device_code",

            _ =>
                "other"
        };
    }
}
