using Microsoft.AspNetCore.TestHost;
using System.Diagnostics.Metrics;
using System.Net;
using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.IntegrationTests;

public sealed class OpenIddictAuthorizationTelemetryTests
{
    [Fact]
    public async Task AuthorizationRequest_RecordsTelemetry()
    {
        using var listener =
            new MeterListener();

        var measurements =
            new List<AuthorizationRequestMeasurement>();

        listener.InstrumentPublished =
            (instrument, meterListener) =>
            {
                if (instrument.Meter.Name ==
                    "Gateway.OpenTelemetry.OpenIddict")
                {
                    meterListener.EnableMeasurementEvents(
                        instrument);
                }
            };

        listener.SetMeasurementEventCallback<long>(
            (instrument, measurement, tags, _) =>
            {
                if (instrument.Name !=
                    "openid_server_authorization_requests_total")
                {
                    return;
                }

                measurements.Add(
                    new AuthorizationRequestMeasurement(
                        measurement,
                        tags.ToArray()));
            });

        listener.Start();

        var application =
            Program.CreateApplication();

        await application.StartAsync();

        try
        {
            var client =
                application.GetTestClient();

            using var response =
                await client.GetAsync(
                    "/connect/authorize" +
                    "?client_id=test-client" +
                    "&response_type=code" +
                    "&redirect_uri=https%3A%2F%2Flocalhost%2Fcallback" +
                    "&scope=openid");

            Assert.NotEqual(
                HttpStatusCode.NotFound,
                response.StatusCode);

            Assert.Contains(
                measurements,
                measurement =>
                    measurement.Value == 1 &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.endpoint",
                        "authorize") &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.result",
                        "received"));
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    private static bool HasTag(
        IReadOnlyList<KeyValuePair<string, object?>> tags,
        string key,
        string expectedValue)
    {
        foreach (var tag in tags)
        {
            if (tag.Key == key &&
                string.Equals(
                    tag.Value?.ToString(),
                    expectedValue,
                    StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private sealed record AuthorizationRequestMeasurement(
        long Value,
        KeyValuePair<string, object?>[] Tags);
}
