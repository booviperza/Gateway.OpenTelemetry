using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Yarp.DependencyInjection;
using Gateway.OpenTelemetry.Yarp.Tracing;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Gateway.OpenTelemetry.Yarp.UnitTests.DependencyInjection;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGatewayYarpOpenTelemetry_Should_Register_YarpTraceEnricher()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        // Act
        services.AddGatewayYarpOpenTelemetry();

        // Assert
        Assert.Contains(
            services,
            descriptor =>
                descriptor.ServiceType == typeof(ITraceEnricher) &&
                descriptor.ImplementationType == typeof(YarpTraceEnricher));
    }

    [Fact]
    public void AddGatewayYarpOpenTelemetry_Should_Return_Same_ServiceCollection()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        // Act
        IServiceCollection result =
            services.AddGatewayYarpOpenTelemetry();

        // Assert
        Assert.Same(
            services,
            result);
    }
}
