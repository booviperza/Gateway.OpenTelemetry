using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.AspNetCore.Tracing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OpenTelemetry.Instrumentation.AspNetCore;

using Xunit;

namespace Gateway.OpenTelemetry.AspNetCore.UnitTests.DependencyInjection;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGatewayOpenTelemetry_Should_Register_TraceInstrumentationOptions()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddGatewayOpenTelemetry();

        Assert.Contains(
            services,
            descriptor =>
                descriptor.ServiceType ==
                typeof(IConfigureOptions<AspNetCoreTraceInstrumentationOptions>));
    }

    [Fact]
    public void AddGatewayOpenTelemetry_Should_Register_ExceptionTraceEnricher()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddGatewayOpenTelemetry();

        Assert.Contains(
            services,
            descriptor =>
                descriptor.ServiceType == typeof(ITraceEnricher) &&
                descriptor.ImplementationType == typeof(ExceptionTraceEnricher));
    }

    [Fact]
    public void AddGatewayOpenTelemetry_Should_Return_Same_ServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();

        IServiceCollection result =
            services.AddGatewayOpenTelemetry();

        Assert.Same(
            services,
            result);
    }
}
