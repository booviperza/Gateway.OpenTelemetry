using Microsoft.AspNetCore.TestHost;
using System.Diagnostics.Metrics;
using System.Net;
using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.IntegrationTests;

public sealed class OpenIddictTokenTelemetryTests
{
    [Fact]
    public async Task TokenRequest_RecordsTelemetry()
    {
        using var listener =
            new MeterListener();

        var measurements =
            new List<TokenRequestMeasurement>();

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
                    "openid_server_token_requests_total")
                {
                    return;
                }

                measurements.Add(
                    new TokenRequestMeasurement(
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

            Assert.NotEqual(
                HttpStatusCode.NotFound,
                response.StatusCode);

            Assert.Contains(
                measurements,
                measurement =>
                    measurement.Value == 1 &&
                    HasTag(
                        measurement.Tags,
                        "openiddict.grant_type",
                        "client_credentials"));
        }
        finally
        {
            await application.StopAsync();

            await application.DisposeAsync();
        }
    }

    [Fact]
    public async Task TokenRequest_InvalidClient_DoesNotExposeSecret()
    {
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
                                "invalid-client",

                            ["client_secret"] =
                                "super-secret-value"
                        }));

            Assert.False(
                response.IsSuccessStatusCode);

            var body =
                await response.Content.ReadAsStringAsync();

            Assert.DoesNotContain(
                "super-secret-value",
                body,
                StringComparison.Ordinal);
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    [Fact]
    public async Task TokenRequest_InvalidClient_RecordsFailureTelemetry()
    {
        using var listener =
            new MeterListener();

        var measurements =
            new List<TokenFailureMeasurement>();

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
                    "openid_server_token_failures_total")
                {
                    return;
                }

                measurements.Add(
                    new TokenFailureMeasurement(
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
                await client.PostAsync(
                    "/connect/token",
                    new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                            ["grant_type"] =
                                "client_credentials",

                            ["client_id"] =
                                "invalid-client",

                            ["client_secret"] =
                                "super-secret-value"
                        }));

            Assert.False(
                response.IsSuccessStatusCode);

            var failure =
                Assert.Single(
                    measurements,
                    measurement =>
                        measurement.Value == 1 &&
                        HasTag(
                            measurement.Tags,
                            "openiddict.grant_type",
                            "client_credentials"));

            Assert.Contains(
                failure.Tags,
                tag =>
                    tag.Key == "openiddict.error" &&
                    !string.IsNullOrWhiteSpace(
                        tag.Value?.ToString()));
        }
        finally
        {
            await application.StopAsync();
            await application.DisposeAsync();
        }
    }

    [Fact]
    public async Task TokenRequest_Success_RecordsIssuedTelemetry()
    {
        using var listener =
            new MeterListener();

        var measurements =
            new List<TokenIssuedMeasurement>();

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
                    "openid_server_tokens_issued_total")
                {
                    return;
                }

                measurements.Add(
                    new TokenIssuedMeasurement(
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

            var issued =
                Assert.Single(
                    measurements,
                    measurement =>
                        measurement.Value == 1 &&
                        HasTag(
                            measurement.Tags,
                            "openiddict.grant_type",
                            "client_credentials") &&
                        HasTag(
                            measurement.Tags,
                            "openiddict.result",
                            "success"));
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

    private sealed record TokenRequestMeasurement(long Value, KeyValuePair<string, object?>[] Tags);

    private sealed record TokenFailureMeasurement(long Value, KeyValuePair<string, object?>[] Tags);

    private sealed record TokenIssuedMeasurement(long Value, KeyValuePair<string, object?>[] Tags);
}
