using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.UnitTests.Security;

public sealed class OpenIddictTelemetryValueNormalizerTests
{
    [Theory]
    [InlineData(
        "authorization_code",
        "authorization_code")]

    [InlineData(
        "refresh_token",
        "refresh_token")]

    [InlineData(
        "client_credentials",
        "client_credentials")]

    [InlineData(
        "password",
        "password")]

    [InlineData(
        "urn:ietf:params:oauth:grant-type:device_code",
        "device_code")]
    public void NormalizeGrantType_KnownValue_ReturnsNormalizedValue(
        string input,
        string expected)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("custom_grant")]
    [InlineData("something-secret")]
    [InlineData("arbitrary-value")]
    public void NormalizeGrantType_UnknownValue_ReturnsOther(
        string input)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(input);

        Assert.Equal("other", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NormalizeGrantType_EmptyValue_ReturnsUnknown(
        string? input)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(input);

        Assert.Equal("unknown", result);
    }
}
