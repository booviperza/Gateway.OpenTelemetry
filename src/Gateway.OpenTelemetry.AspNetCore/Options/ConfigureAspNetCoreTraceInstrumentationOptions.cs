using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Microsoft.AspNetCore.Http;

namespace Gateway.OpenTelemetry.AspNetCore.Options;

/// <summary>
/// Configures ASP.NET Core tracing instrumentation.
/// </summary>
internal sealed class ConfigureAspNetCoreTraceInstrumentationOptions
    : IConfigureOptions<AspNetCoreTraceInstrumentationOptions>
{
    public void Configure(
        AspNetCoreTraceInstrumentationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.EnrichWithHttpResponse = static (activity, response) =>
        {
            ArgumentNullException.ThrowIfNull(activity);
            ArgumentNullException.ThrowIfNull(response);

            HttpContext httpContext = response.HttpContext;

            IEnumerable<ITraceEnricher> enrichers =
                httpContext.RequestServices.GetServices<ITraceEnricher>();

            foreach (ITraceEnricher enricher in enrichers)
            {
                enricher.Enrich(
                    httpContext,
                    activity);
            }
        };
    }
}
