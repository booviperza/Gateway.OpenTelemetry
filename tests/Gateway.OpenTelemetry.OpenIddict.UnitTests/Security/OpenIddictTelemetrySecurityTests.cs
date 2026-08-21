using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.UnitTests.Security;

public sealed class OpenIddictTelemetrySecurityTests
{
    [Theory]
    [InlineData("client-secret-value")]
    [InlineData("access-token-value")]
    [InlineData("refresh-token-value")]
    [InlineData("authorization-code-value")]
    [InlineData("user@example.com")]
    [InlineData("123456789")]
    [InlineData("eyJhbGciOiJIUzI1NiJ9")]
    public void NormalizeGrantType_SensitiveValue_IsNeverReturned(
        string value)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryValueNormalizer
                .NormalizeGrantType(value);

        Assert.NotEqual(value, result);

        Assert.True(
            result is "other" or "unknown");
    }
}
