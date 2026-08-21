using Microsoft.AspNetCore.TestHost;
using System.Diagnostics.Metrics;
using System.Net;
using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.IntegrationTests;

public sealed class OpenIddictRequestDurationTelemetryTests
{
    [Fact]
    public async Task AuthorizationRequest_RecordsRequestDuration()
    {
        using var listener = CreateListener(out var measurements);

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
                    measurement.Value > 0 &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.endpoint",
                        "authorize"));
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    [Fact]
    public async Task AuthorizationRequest_InvalidClient_RecordsRequestDuration()
    {
        using var listener = CreateListener(out var measurements);

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
                    "?client_id=invalid-client" +
                    "&response_type=code" +
                    "&redirect_uri=https%3A%2F%2Flocalhost%2Fcallback" +
                    "&scope=openid");

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);

            Assert.Contains(
                measurements,
                measurement =>
                    measurement.Value > 0 &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.endpoint",
                        "authorize"));
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    [Fact]
    public async Task TokenRequest_Success_RecordsRequestDuration()
    {
        using var listener = CreateListener(out var measurements);

        var application =
            Program.CreateApplication();

        await application.StartAsync();

        try
        {
            var client =
                application.GetTestClient();

            using var response =
                await client.PostAsync(
                    "/connect/token",
                    new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                            ["grant_type"] =
                                "client_credentials",

                            ["client_id"] =
                                "test-client",

                            ["client_secret"] =
                                "test-secret"
                        }));

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            Assert.Contains(
                measurements,
                measurement =>
                    measurement.Value > 0 &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.endpoint",
                        "token"));
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    private static MeterListener CreateListener(out List<RequestDurationMeasurement> measurements)
    {
        var capturedMeasurements =
            new List<RequestDurationMeasurement>();

        measurements = capturedMeasurements;

        var listener = new MeterListener();

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

        listener.SetMeasurementEventCallback<double>(
            (instrument, measurement, tags, _) =>
            {
                if (instrument.Name !=
                    "openid_server_request_duration_seconds")
                {
                    return;
                }

                capturedMeasurements.Add(
                    new RequestDurationMeasurement(
                        measurement,
                        tags.ToArray()));
            });

        listener.Start();

        return listener;
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

    private sealed record RequestDurationMeasurement(
        double Value,
        KeyValuePair<string, object?>[] Tags);
}
