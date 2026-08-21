using Gateway.OpenTelemetry.OpenIddict.Integration.Token;
using Gateway.OpenTelemetry.OpenIddict.Metrics;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Diagnostics.Metrics;
using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.UnitTests.Integration.Token;

public sealed class TokenIssuedTelemetryHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenAccessTokenIsGenerated_RecordsIssuedTelemetry()
    {
        using var meter = OpenIddictMeter.Create();

        var metrics = new OpenIddictMetrics(meter);

        var recorder = new OpenIddictTelemetryRecorder(metrics);

        var handler = new TokenIssuedTelemetryHandler(recorder);

        var measurements = new List<Measurement<long>>();

        using var listener = new MeterListener();

        listener.InstrumentPublished =
            (instrument, meterListener) =>
            {
                if (instrument.Meter == meter &&
                    instrument.Name ==
                        OpenIddictMetricNames.TokenIssued)
                {
                    meterListener.EnableMeasurementEvents(
                        instrument);
                }
            };

        listener.SetMeasurementEventCallback<long>(
            (instrument, measurement, tags, _) =>
            {
                measurements.Add(
                    new Measurement<long>(
                        measurement,
                        tags));
            });

        listener.Start();

        var context =
            CreateContext(
                generateAccessToken: true,
                grantType: "client_credentials");

        await handler.HandleAsync(context);

        listener.RecordObservableInstruments();

        var measurement =
            Assert.Single(measurements);

        Assert.Equal(1, measurement.Value);

        Assert.Equal(
            "client_credentials",
            GetTag(
                measurement.Tags,
                OpenIddictTagNames.GrantType));

        Assert.Equal(
            OpenIddictTelemetryResults.Success,
            GetTag(
                measurement.Tags,
                OpenIddictTagNames.Result));
    }

    [Fact]
    public async Task HandleAsync_WhenAccessTokenIsNotGenerated_DoesNotRecordIssuedTelemetry()
    {
        using var meter = OpenIddictMeter.Create();

        var metrics = new OpenIddictMetrics(meter);

        var recorder = new OpenIddictTelemetryRecorder(metrics);

        var handler = new TokenIssuedTelemetryHandler(recorder);

        var measurements = new List<Measurement<long>>();

        using var listener = new MeterListener();

        listener.InstrumentPublished =
            (instrument, meterListener) =>
            {
                if (instrument.Meter == meter &&
                    instrument.Name ==
                        OpenIddictMetricNames.TokenIssued)
                {
                    meterListener.EnableMeasurementEvents(
                        instrument);
                }
            };

        listener.SetMeasurementEventCallback<long>(
            (instrument, measurement, tags, _) =>
            {
                measurements.Add(
                    new Measurement<long>(
                        measurement,
                        tags));
            });

        listener.Start();

        var context =
            CreateContext(
                generateAccessToken: false,
                grantType: "client_credentials");

        await handler.HandleAsync(context);

        listener.RecordObservableInstruments();

        Assert.Empty(measurements);
    }

    private static OpenIddictServerEvents.ProcessSignInContext
        CreateContext(
            bool generateAccessToken,
            string grantType)
    {
        var transaction =
            new OpenIddictServerTransaction();

        var request =
            new OpenIddictRequest
            {
                GrantType = grantType
            };

        transaction.Request = request;

        return new OpenIddictServerEvents.ProcessSignInContext(
            transaction)
        {
            GenerateAccessToken = generateAccessToken
        };
    }

    private static string? GetTag(
        ReadOnlySpan<KeyValuePair<string, object?>> tags,
        string name)
    {
        foreach (var tag in tags)
        {
            if (string.Equals(
                    tag.Key,
                    name,
                    StringComparison.Ordinal))
            {
                return tag.Value?.ToString();
            }
        }

        return null;
    }
}
