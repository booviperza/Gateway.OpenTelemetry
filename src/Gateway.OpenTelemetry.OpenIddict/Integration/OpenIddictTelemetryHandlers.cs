using Gateway.OpenTelemetry.OpenIddict.Integration.Authorization;
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

    public static OpenIddictServerHandlerDescriptor AuthorizationRequest
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ValidateAuthorizationRequestContext>()
            .UseSingletonHandler<
                AuthorizationRequestTelemetryHandler>()
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
            .SetOrder(
                OpenIddictServerHandlers
                    .EvaluateGeneratedTokens
                    .Descriptor
                    .Order + 1)
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

    public static OpenIddictServerHandlerDescriptor AuthorizationDenied
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessErrorContext>()
            .UseSingletonHandler<
                AuthorizationDeniedTelemetryHandler>()
            .SetOrder(100_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor RequestDuration
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessRequestContext>()
            .UseSingletonHandler<
                OpenIddictRequestDurationTelemetryHandler>()
            .SetOrder(100_001)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor AuthorizationResponseDuration
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ApplyAuthorizationResponseContext>()
            .UseSingletonHandler<
                Authorization.AuthorizationResponseDurationTelemetryHandler>()
            .SetOrder(499_999)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor RequestDurationError
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ProcessErrorContext>()
            .UseSingletonHandler<
                OpenIddictRequestDurationErrorTelemetryHandler>()
            .SetOrder(100_001)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public static OpenIddictServerHandlerDescriptor TokenResponseDuration
        => OpenIddictServerHandlerDescriptor
            .CreateBuilder<
                OpenIddictServerEvents.ApplyTokenResponseContext>()
            .UseSingletonHandler<
                TokenResponseDurationTelemetryHandler>()
            .SetOrder(100_001)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();
}
