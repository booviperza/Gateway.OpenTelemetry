using Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

using System.Diagnostics;
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
        // Arrange
        _fixture.Collector.Clear();

        // Act
        HttpResponseMessage response =
            await _fixture.Client.GetAsync("/ping");

        // Assert HTTP response
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        // Assert Activity
        Activity? activity =
            _fixture.Collector.LastActivity;

        Assert.NotNull(activity);

        // /ping is an ASP.NET Core endpoint, not a YARP request.
        // Therefore YARP-specific tags must not be present.
        Assert.Null(
            activity.GetTagItem(
                "gateway.yarp.route_id"));

        Assert.Null(
            activity.GetTagItem(
                "gateway.yarp.cluster_id"));

        Assert.Null(
            activity.GetTagItem(
                "gateway.yarp.destination_id"));
    }

    [Fact]
    public async Task Proxy_Should_Reach_Backend()
    {
        // Arrange
        _fixture.Collector.Clear();

        // Act
        HttpResponseMessage response =
            await _fixture.Client.GetAsync("/proxy/ping");

        response.EnsureSuccessStatusCode();

        string body =
            await response.Content.ReadAsStringAsync();

        // Assert HTTP response
        Assert.Equal(
            "pong",
            body);

        // Assert Activity
        Activity? activity =
            _fixture.Collector.LastActivity;

        Assert.NotNull(activity);

        // Assert YARP Route
        string? routeId =
            activity.GetTagItem(
                "gateway.yarp.route_id") as string;

        Assert.False(
            string.IsNullOrWhiteSpace(routeId));

        // Assert YARP Cluster
        string? clusterId =
            activity.GetTagItem(
                "gateway.yarp.cluster_id") as string;

        Assert.False(
            string.IsNullOrWhiteSpace(clusterId));

        // Assert YARP Destination
        string? destinationId =
            activity.GetTagItem(
                "gateway.yarp.destination_id") as string;

        Assert.False(
            string.IsNullOrWhiteSpace(destinationId));
    }

    [Fact]
    public async Task Proxy_Should_Enrich_Http_Metric_Tags()
    {
        // Arrange
        _fixture.MetricTags.Clear();

        // Act
        HttpResponseMessage response =
            await _fixture.Client.GetAsync("/proxy/ping");

        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(
            "cluster1",
            _fixture.MetricTags.GetValue(
                "gateway.yarp.cluster_id"));

        Assert.Equal(
            "backend1",
            _fixture.MetricTags.GetValue(
                "gateway.yarp.destination_id"));
    }
}
