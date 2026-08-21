namespace Gateway.OpenTelemetry.OpenIddict.Metrics;

/// <summary>
/// Defines metric names used by the OpenIddict telemetry integration.
/// </summary>
internal static class OpenIddictMetricNames
{
    public const string ServerRequests = "openid_server_requests_total";

    public const string ServerRequestDuration =
        "openid_server_request_duration_seconds";

    public const string TokenRequests =
        "openid_server_token_requests_total";

    public const string TokenIssued =
        "openid_server_tokens_issued_total";

    public const string TokenFailures =
        "openid_server_token_failures_total";

    public const string AuthorizationRequests =
        "openid_server_authorization_requests_total";

    public const string AuthorizationDenied =
        "openid_server_authorization_denied_total";
}
