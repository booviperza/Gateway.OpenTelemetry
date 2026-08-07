using Gateway.OpenTelemetry.AspNetCore.Tracing;
using Gateway.OpenTelemetry.Core.Constants;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Xunit;

namespace Gateway.OpenTelemetry.AspNetCore.UnitTests.Tracing;

public sealed class ExceptionTraceEnricherTests
{
    [Fact]
    public void Enrich_Should_Not_Add_Tag_When_Exception_Does_Not_Exist()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        ExceptionTraceEnricher enricher = new();

        using Activity activity = new("test");

        activity.Start();

        // Act
        enricher.Enrich(
            httpContext,
            activity);

        // Assert
        Assert.Null(
            activity.GetTagItem(
                GatewayTagNames.ExceptionType));
    }

    [Fact]
    public void Enrich_Should_Add_Exception_Type_Tag()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        InvalidOperationException exception =
            new("boom");

        httpContext.Features.Set<IExceptionHandlerFeature>(
            new ExceptionHandlerFeature
            {
                Error = exception
            });

        ExceptionTraceEnricher enricher = new();

        using Activity activity = new("test");

        activity.Start();

        // Act
        enricher.Enrich(
            httpContext,
            activity);

        // Assert
        Assert.Equal(
            typeof(InvalidOperationException).FullName,
            activity.GetTagItem(
                GatewayTagNames.ExceptionType));
    }
}
