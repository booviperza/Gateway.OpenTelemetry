using Gateway.OpenTelemetry.OpenIddict.Integration.Token;
using OpenIddict.Server;

namespace Gateway.OpenTelemetry.OpenIddict.Integration;

/// <summary>
/// Provides the descriptors used to register
/// Gateway.OpenTelemetry OpenIddict handlers.
/// </summary>
internal static class OpenIddictTelemetryHandlers
{
    public static OpenIddictServerHandlerDescriptor Request
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessRequestContext>()
            .UseSingletonHandler<
                OpenIddictRequestTelemetryHandler>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor TokenRequest
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ValidateTokenRequestContext>()
            .UseSingletonHandler<
                TokenRequestTelemetryHandler>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor TokenIssued
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessSignInContext>()
            .UseSingletonHandler<
                TokenIssuedTelemetryHandler>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor TokenFailure
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessErrorContext>()
            .UseSingletonHandler<
                TokenFailureTelemetryHandler>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();
}
