using Gateway.OpenTelemetry.Core.Constants;

using Xunit;

namespace Gateway.OpenTelemetry.Core.UnitTests.Constants;

public sealed class GatewayTagNamesTests
{
    [Fact]
    public void Host_Should_Have_Expected_Value()
    {
        Assert.Equal(
            "gateway.host",
            GatewayTagNames.Host);
    }

    [Fact]
    public void ExceptionType_Should_Have_Expected_Value()
    {
        Assert.Equal(
            "gateway.exception.type",
            GatewayTagNames.ExceptionType);
    }

    [Fact]
    public void YarpRouteId_Should_Have_Expected_Value()
    {
        Assert.Equal(
            "gateway.yarp.route_id",
            GatewayTagNames.YarpRouteId);
    }

    [Fact]
    public void YarpClusterId_Should_Have_Expected_Value()
    {
        Assert.Equal(
            "gateway.yarp.cluster_id",
            GatewayTagNames.YarpClusterId);
    }

    [Fact]
    public void YarpDestinationId_Should_Have_Expected_Value()
    {
        Assert.Equal(
            "gateway.yarp.destination_id",
            GatewayTagNames.YarpDestinationId);
    }
}
