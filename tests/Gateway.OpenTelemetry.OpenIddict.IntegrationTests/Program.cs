using Gateway.OpenTelemetry.OpenIddict.DependencyInjection;
using Gateway.OpenTelemetry.OpenIddict.Integration;
using Microsoft.AspNetCore.TestHost;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
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

                options.SetAuthorizationEndpointUris("/connect/authorize");

                options.AllowClientCredentialsFlow();

                options.AllowAuthorizationCodeFlow();

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
                options.AddEventHandler<ValidateTokenRequestContext>(
                    builder =>
                    {
                        builder.UseInlineHandler(context =>
                        {
                            if (!context.Request
                                    .IsClientCredentialsGrantType())
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
                                    description:
                                        "The specified client is invalid.");

                                return default;
                            }

                            if (!string.Equals(
                                    context.ClientSecret,
                                    "test-secret",
                                    StringComparison.Ordinal))
                            {
                                context.Reject(
                                    error: Errors.InvalidClient,
                                    description:
                                        "The specified client credentials are invalid.");

                                return default;
                            }

                            return default;
                        });
                    });

                // Test-only token request handler.
                options.AddEventHandler<HandleTokenRequestContext>(
                    builder =>
                    {
                        builder.UseInlineHandler(context =>
                        {
                            if (!context.Request
                                    .IsClientCredentialsGrantType())
                            {
                                return default;
                            }

                            var identity =
                                new ClaimsIdentity(
                                    authenticationType:
                                        OpenIddictServerAspNetCoreDefaults
                                            .AuthenticationScheme);

                            identity.AddClaim(
                                Claims.Subject,
                                context.ClientId ?? "test-client");

                            var principal =
                                new ClaimsPrincipal(identity);

                            context.SignIn(principal);

                            return default;
                        });
                    });

                // Test-only authorization request handler.
                //
                // This handler only handles the request so the
                // integration test can exercise the authorization
                // validation pipeline. No credentials or tokens are
                // written to telemetry.
                // Test-only authorization request validation.
                options.AddEventHandler<ValidateAuthorizationRequestContext>(
                    builder =>
                    {
                        builder.UseInlineHandler(context =>
                        {
                            if (!string.Equals(
                                    context.ClientId,
                                    "test-client",
                                    StringComparison.Ordinal))
                            {
                                context.Reject(
                                    error: Errors.InvalidClient,
                                    description:
                                        "The specified client is invalid.");

                                return default;
                            }

                            return default;
                        });
                    });

                // Test-only authorization response.
                options.AddEventHandler<HandleAuthorizationRequestContext>(
                    builder =>
                    {
                        builder.UseInlineHandler(context =>
                        {
                            var identity =
                                new ClaimsIdentity(
                                    authenticationType:
                                        OpenIddictServerAspNetCoreDefaults
                                            .AuthenticationScheme);

                            identity.AddClaim(
                                Claims.Subject,
                                context.ClientId ?? "test-client");

                            var principal =
                                new ClaimsPrincipal(identity);

                            context.SignIn(principal);

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
