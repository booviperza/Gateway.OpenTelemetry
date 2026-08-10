using Gateway.OpenTelemetry.AspNetCore.Tracing;

using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.AspNetCore.Options;

/// <summary>
/// Configures ASP.NET Core tracing instrumentation.
/// </summary>
internal sealed class ConfigureAspNetCoreTraceInstrumentationOptions
    : IConfigureOptions<AspNetCoreTraceInstrumentationOptions>
{
    private readonly CompositeTraceEnricher _compositeTraceEnricher;

    public ConfigureAspNetCoreTraceInstrumentationOptions(
        CompositeTraceEnricher compositeTraceEnricher)
    {
        ArgumentNullException.ThrowIfNull(compositeTraceEnricher);

        _compositeTraceEnricher = compositeTraceEnricher;
    }

    public void Configure(
        AspNetCoreTraceInstrumentationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.EnrichWithHttpResponse = (activity, response) =>
        {
            ArgumentNullException.ThrowIfNull(activity);
            ArgumentNullException.ThrowIfNull(response);

            HttpContext httpContext = response.HttpContext;

            _compositeTraceEnricher.Enrich(
                httpContext,
                activity);
        };
    }
}
