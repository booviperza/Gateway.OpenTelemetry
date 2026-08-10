namespace Gateway.OpenTelemetry.Core.Metrics;

/// <summary>
/// Defines metric tag names used by Gateway.OpenTelemetry.
/// </summary>
public static class GatewayMetricTagNames
{
    /// <summary>
    /// HTTP request method.
    /// </summary>
    public const string HttpRequestMethod =
        "http.request.method";

    /// <summary>
    /// HTTP response status code.
    /// </summary>
    public const string HttpResponseStatusCode =
        "http.response.status_code";

    /// <summary>
    /// HTTP route template.
    /// </summary>
    public const string HttpRoute =
        "http.route";
}
