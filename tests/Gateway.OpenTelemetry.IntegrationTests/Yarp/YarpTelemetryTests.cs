using Gateway.OpenTelemetry.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Gateway.OpenTelemetry.IntegrationTests.Yarp;

/// <summary>
/// End-to-end tests for YARP integration.
/// </summary>
public sealed class YarpTelemetryTests
    : IClassFixture<GatewayFixture>
{
    private readonly GatewayFixture _fixture;

    public YarpTelemetryTests(
        GatewayFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Ping_Should_Return_OK()
    {
        HttpResponseMessage response =
            await _fixture.Client.GetAsync("/ping");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Proxy_Should_Reach_Backend()
    {
        HttpResponseMessage response =
            await _fixture.Client.GetAsync("/proxy/ping");

        response.EnsureSuccessStatusCode();

        string body =
            await response.Content.ReadAsStringAsync();

        Assert.Equal(
            "pong",
            body);
    }
}
