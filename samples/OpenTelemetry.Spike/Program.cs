using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.Yarp.DependencyInjection;

using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });

builder.Services
    .AddGatewayOpenTelemetry();

builder.Services
    .AddGatewayYarpOpenTelemetry();

WebApplication app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello OpenTelemetry";
});

app.MapGet("/hello/{name}", (string name) =>
{
    return Results.Ok($"Hello {name}");
});

app.MapGet("/error", () =>
{
    throw new InvalidOperationException("Spike Exception");
});

app.MapGet("/ping", () => Results.Ok());

app.MapReverseProxy();

app.Run();

public partial class Program;
