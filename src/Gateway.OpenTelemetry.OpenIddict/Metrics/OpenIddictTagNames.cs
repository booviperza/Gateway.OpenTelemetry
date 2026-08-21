namespace Gateway.OpenTelemetry.OpenIddict.Metrics;

/// <summary>
/// Defines the allowed OpenIddict telemetry tag names.
/// </summary>
internal static class OpenIddictTagNames
{
    public const string Endpoint =
        "openiddict.endpoint";

    public const string GrantType =
        "openiddict.grant_type";

    public const string Result =
        "openiddict.result";

    public const string Error =
        "openiddict.error";

    public const string HttpMethod =
        "openiddict.http.method";

    public const string HttpStatusCode =
        "openiddict.http.status_code";
}
