using Gateway.OpenTelemetry.Core.Constants;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.Tracing;

/// <summary>
/// Adds gateway specific exception tags.
/// </summary>
internal sealed class ExceptionTraceEnricher : ITraceEnricher
{
    public void Enrich(
        HttpContext httpContext,
        Activity activity)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(activity);

        IExceptionHandlerFeature? feature =
            httpContext.Features.Get<IExceptionHandlerFeature>();

        Exception? exception = feature?.Error;

        if (exception is null)
        {
            return;
        }

        activity.SetTag(
            GatewayTagNames.ExceptionType,
            exception.GetType().FullName);
    }
}
