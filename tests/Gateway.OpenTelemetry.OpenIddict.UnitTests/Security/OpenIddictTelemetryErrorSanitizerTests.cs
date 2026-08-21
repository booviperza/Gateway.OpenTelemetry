using Xunit;

namespace Gateway.OpenTelemetry.OpenIddict.UnitTests.Security;

public sealed class OpenIddictTelemetryErrorSanitizerTests
{
    [Theory]
    [InlineData("invalid_client")]
    [InlineData("invalid_grant")]
    [InlineData("invalid_request")]
    [InlineData("invalid_scope")]
    [InlineData("unauthorized_client")]
    [InlineData("unsupported_grant_type")]
    [InlineData("unsupported_response_type")]
    [InlineData("access_denied")]
    [InlineData("server_error")]
    [InlineData("temporarily_unavailable")]
    public void Sanitize_AllowedError_ReturnsSameValue(
        string error)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryErrorSanitizer
                .Sanitize(error);

        Assert.Equal(error, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Sanitize_EmptyError_ReturnsNull(
        string? error)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryErrorSanitizer
                .Sanitize(error);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("secret-token")]
    [InlineData("refresh_token=very-secret-value")]
    [InlineData("client_secret=very-secret-value")]
    [InlineData("authorization_code=very-secret-value")]
    [InlineData("password=very-secret-value")]
    [InlineData("Bearer eyJhbGciOi...")]
    [InlineData("some-internal-exception")]
    public void Sanitize_UnknownError_ReturnsUnknown(
        string error)
    {
        var result =
            global::Gateway.OpenTelemetry.OpenIddict.Security
                .OpenIddictTelemetryErrorSanitizer
                .Sanitize(error);

        Assert.Equal("unknown", result);
    }
}
