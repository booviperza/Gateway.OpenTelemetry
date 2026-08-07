using Gateway.OpenTelemetry.AspNetCore.Options;
using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.AspNetCore.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using System.Diagnostics;
using Xunit;

namespace Gateway.OpenTelemetry.AspNetCore.UnitTests.Options;

public sealed class ConfigureAspNetCoreTraceInstrumentationOptionsTests
{
    [Fact]
    public void Configure_Should_Invoke_All_TraceEnrichers()
    {
        // Arrange
        ServiceCollection services = new();

        FakeTraceEnricher fake = new();

        services.AddSingleton<ITraceEnricher>(fake);

        ServiceProvider provider = services.BuildServiceProvider();

        DefaultHttpContext httpContext = new();

        httpContext.RequestServices = provider;

        AspNetCoreTraceInstrumentationOptions options = new();

        ConfigureAspNetCoreTraceInstrumentationOptions configure = new();

        configure.Configure(options);

        using Activity activity = new("test");

        activity.Start();

        // Act
        options.EnrichWithHttpResponse!(
            activity,
            httpContext.Response);

        // Assert
        Assert.Equal(
            1,
            fake.CallCount);

        Assert.Same(
            httpContext,
            fake.LastHttpContext);

        Assert.Same(
            activity,
            fake.LastActivity);
    }
}
