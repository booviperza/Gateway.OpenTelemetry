using Gateway.OpenTelemetry.OpenIddict.DependencyInjection;
using Gateway.OpenTelemetry.OpenIddict.Integration;
using Microsoft.AspNetCore.TestHost;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Gateway.OpenTelemetry.OpenIddict.IntegrationTests;

public static class Program
{
    public static WebApplication CreateApplication()
    {
        var builder =
            WebApplication.CreateBuilder();

        builder.WebHost.UseTestServer();

        builder.Services
            .AddGatewayOpenIddictOpenTelemetry();

        builder.Services
            .AddOpenIddict()
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token");

                options.AllowClientCredentialsFlow();

                options.EnableDegradedMode();

                options.AddEphemeralEncryptionKey();
                options.AddEphemeralSigningKey();

                // Gateway.OpenTelemetry must be registered
                // while the builder is still OpenIddictServerBuilder.
                options.UseGatewayOpenTelemetry();

                options
                    .UseAspNetCore()
                    .DisableTransportSecurityRequirement();

                // Test-only client authentication.
                //
                // IMPORTANT:
                // - The client secret is only used for validation.
                // - It must never be added to Activity tags, metrics,
                //   logs, or any telemetry payload.
                options.AddEventHandler<ValidateTokenRequestContext>(builder =>
                {
                    builder.UseInlineHandler(context =>
                    {
                        if (!context.Request.IsClientCredentialsGrantType())
                        {
                            return default;
                        }

                        if (!string.Equals(
                                context.ClientId,
                                "test-client",
                                StringComparison.Ordinal))
                        {
                            context.Reject(
                                error: Errors.InvalidClient,
                                description: "The specified client is invalid.");

                            return default;
                        }

                        if (!string.Equals(
                                context.ClientSecret,
                                "test-secret",
                                StringComparison.Ordinal))
                        {
                            context.Reject(
                                error: Errors.InvalidClient,
                                description: "The specified client credentials are invalid.");

                            return default;
                        }

                        return default;
                    });
                });

                // Test-only token request handler.
                options.AddEventHandler<HandleTokenRequestContext>(builder =>
                {
                    builder.UseInlineHandler(context =>
                    {
                        context.HandleRequest();

                        return default;
                    });
                });
            });

        var app =
            builder.Build();

        app.MapGet(
            "/health",
            () => Results.Ok(
                new
                {
                    status = "ok"
                }));

        return app;
    }
}
