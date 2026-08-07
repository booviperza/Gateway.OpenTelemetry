using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Gateway.OpenTelemetry.AspNetCore.UnitTests.TestDoubles;

internal sealed class FakeTraceEnricher : ITraceEnricher
{
    public int CallCount { get; private set; }

    public HttpContext? LastHttpContext { get; private set; }

    public Activity? LastActivity { get; private set; }

    public void Enrich(
        HttpContext httpContext,
        Activity activity)
    {
        CallCount++;

        LastHttpContext = httpContext;

        LastActivity = activity;
    }
}
