using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;



namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// In-memory backend server used by integration tests.
/// Runs on Kestrel using a dynamically assigned localhost port.
/// </summary>
internal sealed class BackendHost : IAsyncDisposable
{
    private readonly WebApplication _application;

    private BackendHost(WebApplication application)
    {
        _application = application;
    }

    /// <summary>
    /// Gets the backend base address.
    /// </summary>
    public Uri BaseAddress
    {
        get
        {
            IServer server = _application.Services.GetRequiredService<IServer>();

            IServerAddressesFeature feature =
                server.Features.Get<IServerAddressesFeature>()!;

            string address = feature.Addresses.Single();

            return new Uri(address);
        }
    }

    /// <summary>
    /// Starts the backend server.
    /// </summary>
    public static async Task<BackendHost> StartAsync()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.WebHost.UseUrls("http://127.0.0.1:0");

        WebApplication app = builder.Build();

        app.MapGet(
            "/ping",
            () => Results.Text("pong"));

        await app.StartAsync();

        return new BackendHost(app);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _application.StopAsync();

        await _application.DisposeAsync();
    }
}
