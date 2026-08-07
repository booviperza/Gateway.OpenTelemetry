using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Shared fixture for Gateway integration tests.
/// </summary>
public sealed class GatewayFixture
    : WebApplicationFactory<Program>
{
    private BackendHost? _backendHost;
    public GatewayFixture()
    {
        _backendHost = BackendHost
            .StartAsync()
            .GetAwaiter()
            .GetResult();

        Client = CreateClient();
    }
    /// <summary>
    /// Gets the shared HTTP client.
    /// </summary>
    public HttpClient Client { get; private set; } = default!;

    /// <summary>
    /// Gets collected activities.
    /// </summary>
    internal ActivityCollector Collector { get; } = new();

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        Client?.Dispose();

        if (_backendHost is not null)
        {
            await _backendHost.DisposeAsync();
        }

        await base.DisposeAsync();
    }

    /// <inheritdoc />
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            if (_backendHost is null)
            {
                return;
            }

            Dictionary<string, string?> settings = new()
            {
                ["ReverseProxy:Clusters:cluster1:Destinations:backend1:Address"]
                    = _backendHost.BaseAddress.ToString()
            };

            configuration.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(_ =>
        {
            // STEP ถัดไป
            // Inject TestActivityExporter
        });
    }
}
