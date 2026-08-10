using Gateway.OpenTelemetry.Yarp.Metrics;
using Microsoft.AspNetCore.Http;
using Xunit;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Model;

namespace Gateway.OpenTelemetry.Yarp.UnitTests.Metrics;

public sealed class YarpMetricEnricherTests
{
    [Fact]
    public void Enrich_Should_Do_Nothing_When_ProxyFeature_Is_Missing()
    {
        DefaultHttpContext httpContext = new();

        List<KeyValuePair<string, object?>> tags = [];

        YarpMetricEnricher enricher = new();

        enricher.Enrich(
            httpContext,
            tags);

        Assert.Empty(tags);
    }

    [Fact]
    public void Enrich_Should_Add_Route_Cluster_And_Destination()
    {
        DefaultHttpContext httpContext =
            CreateHttpContext(
                routeId: "route1",
                clusterId: "cluster1",
                destinationId: "backend1");

        List<KeyValuePair<string, object?>> tags = [];

        YarpMetricEnricher enricher = new();

        enricher.Enrich(
            httpContext,
            tags);

        Assert.Equal(
            "route1",
            GetValue(
                tags,
                "gateway.yarp.route_id"));

        Assert.Equal(
            "cluster1",
            GetValue(
                tags,
                "gateway.yarp.cluster_id"));

        Assert.Equal(
            "backend1",
            GetValue(
                tags,
                "gateway.yarp.destination_id"));
    }

    [Fact]
    public void Enrich_Should_Add_Route_And_Cluster_When_Destination_Is_Missing()
    {
        DefaultHttpContext httpContext =
            CreateHttpContext(
                routeId: "route1",
                clusterId: "cluster1",
                destinationId: null);

        List<KeyValuePair<string, object?>> tags = [];

        YarpMetricEnricher enricher = new();

        enricher.Enrich(
            httpContext,
            tags);

        Assert.Equal(
            "route1",
            GetValue(
                tags,
                "gateway.yarp.route_id"));

        Assert.Equal(
            "cluster1",
            GetValue(
                tags,
                "gateway.yarp.cluster_id"));

        Assert.Null(
            GetValue(
                tags,
                "gateway.yarp.destination_id"));
    }

    private static DefaultHttpContext CreateHttpContext(
    string? routeId,
    string? clusterId,
    string? destinationId)
    {
        ClusterModel cluster = null!;

        if (clusterId is not null)
        {
            ClusterConfig clusterConfig = new()
            {
                ClusterId = clusterId
            };

            cluster =
                new ClusterModel(
                    clusterConfig,
                    new HttpMessageInvoker(
                        new HttpClientHandler()));
        }

        RouteModel route = null!;

        if (routeId is not null)
        {
            RouteConfig routeConfig = new()
            {
                RouteId = routeId,
                ClusterId = clusterId
            };

            route =
                new RouteModel(
                    routeConfig,
                    null,
                    HttpTransformer.Default);
        }

        DestinationState? destination = null;

        if (destinationId is not null)
        {
            DestinationConfig destinationConfig = new()
            {
                Address = "http://localhost/"
            };

            DestinationModel destinationModel =
                new(destinationConfig);

            destination =
                new DestinationState(
                    destinationId,
                    destinationModel);
        }

        ReverseProxyFeature feature =
            new()
            {
                Route = route,
                Cluster = cluster,
                ProxiedDestination = destination
            };

        DefaultHttpContext context = new();

        context.Features.Set<IReverseProxyFeature>(
            feature);

        return context;
    }

    private static object? GetValue(
        IEnumerable<KeyValuePair<string, object?>> tags,
        string name)
    {
        return tags
            .FirstOrDefault(
                tag => tag.Key == name)
            .Value;
    }
}
