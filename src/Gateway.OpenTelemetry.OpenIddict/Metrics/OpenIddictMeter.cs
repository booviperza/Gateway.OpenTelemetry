using System.Diagnostics.Metrics;

namespace Gateway.OpenTelemetry.OpenIddict.Metrics;

/// <summary>
/// Provides the Meter used by Gateway.OpenTelemetry.OpenIddict.
/// </summary>
internal static class OpenIddictMeter
{
    public const string Name =
        "Gateway.OpenTelemetry.OpenIddict";

    public static Meter Create()
    {
        return new Meter(Name);
    }
}
